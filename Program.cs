using RedisDemoAPI;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger (THIS is the correct way)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false"));

// Fake DB
builder.Services.AddSingleton<FakeProductRepository>();

var app = builder.Build();

// Enable Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();   // <-- THIS SHOWS THE SWAGGER UI
}

app.UseHttpsRedirection();

// Redis endpoint  
app.MapGet("/product/{id:int}", async (int id,
    IConnectionMultiplexer redis,
    FakeProductRepository repo) =>
{
    var db = redis.GetDatabase();
    string key = $"product:{id}:price";

    var cached = await db.StringGetAsync(key);

    if (cached.HasValue)
        return Results.Ok(new { ProductId = id, Price = cached.ToString(), Source = "Redis Cache" });

    var price = await repo.GetPriceFromDbAsync(id);

    await db.StringSetAsync(key, price.ToString(), TimeSpan.FromSeconds(30));

    return Results.Ok(new { ProductId = id, Price = price, Source = "Database (now cached)" });
});

// Clear cache endpoint  
app.MapDelete("/product/{id:int}/cache", async (int id,
    IConnectionMultiplexer redis) =>
{
    var db = redis.GetDatabase();
    await db.KeyDeleteAsync($"product:{id}:price");
    return Results.Ok($"Cache cleared for product {id}");
});

app.MapGet("/", () => "API is running...");

app.Run();
