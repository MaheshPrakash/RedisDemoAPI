🚀 .NET 9 Redis Caching Demo (Minimal API + Docker + StackExchange.Redis)

This project demonstrates how to implement high-performance caching using Redis inside a .NET 9 Minimal API, with Redis running inside a Docker container. The API simulates a slow database call (3 seconds) and then uses Redis to drastically improve performance by returning cached responses instantly.

📘 Features

✔ .NET 9 Minimal API
✔ Redis caching using StackExchange.Redis
✔ Docker-based Redis server
✔ Swagger UI enabled
✔ Fake DB simulation with delay
✔ TTL (Time-To-Live) caching (30 seconds)
✔ Cache invalidation endpoint
✔ Clean, simple architecture

🏗️ Architecture Overview
Client (Postman / Swagger)
        |
        v
.NET 9 Minimal API
        |
        v
Check Redis Cache  <───┐
        |               |
        | Cache Miss    | Cache Hit
        v               |
Fake DB (3s delay)      |
        |               |
        v               |
Store in Redis ─────────┘

🔧 Technologies Used
Technology	Purpose
.NET 9 Minimal API	Backend API framework
Docker	Running Redis locally
Redis	High-speed in-memory caching
StackExchange.Redis	.NET Redis Client
Swagger / Swashbuckle	API documentation & testing
🐳 Run Redis Using Docker

Make sure Docker Desktop is installed and running.

Start Redis container:
docker run --name redis -d -p 6379:6379 redis

Verify:
docker ps

Optional: Test Redis CLI:
docker exec -it redis redis-cli
ping
→ PONG

▶️ Run the .NET 9 Application
dotnet run


The API starts on:

https://localhost:<PORT>


Open Swagger UI:

https://localhost:<PORT>/swagger

🔥 API Endpoints
✔ GET /product/{id}

Retrieves a product’s price.
The first request is slow (from fake DB), later requests are instant (from Redis).

First Call (Slow, DB Fetch)
{
  "productId": 1,
  "price": 101,
  "source": "Database (now cached)"
}

Second Call (Instant, Redis Cache)
{
  "productId": 1,
  "price": "101",
  "source": "Redis Cache"
}

✔ DELETE /product/{id}/cache

Manually clears cached value.

"Cache cleared for product 1"

⏳ Cache Expiration (TTL)

Cache expires after 30 seconds:

await db.StringSetAsync(key, price.ToString(), TimeSpan.FromSeconds(30));


After expiration, the next request fetches from DB again.

📁 Project Structure
RedisDemoAPI/
│── Program.cs
│── FakeProductRepository.cs
│── RedisDemoAPI.csproj
│── appsettings.json
│── Properties/
│── README.md
│── .gitignore

🧩 Key Code Snippets
✔ Redis Registration
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false"));

✔ Caching Logic
var cached = await db.StringGetAsync(key);

if (cached.HasValue)
    return Results.Ok(new { ProductId = id, Price = cached.ToString(), Source = "Redis Cache" });

🛠️ How to Test Using Postman
1️⃣ First Request (Cache Miss)
GET https://localhost:<PORT>/product/1


Returns from database and saves to Redis.

2️⃣ Second Request (Cache Hit)
GET https://localhost:<PORT>/product/1


Returns instantly from Redis.

3️⃣ Clear cache
DELETE https://localhost:<PORT>/product/1/cache

⭐ Improvements / Next Steps

You can extend this project with:

Redis Hashes (store entire objects)

Redis Lists / Sets

Redis Pub/Sub messaging

Redis JSON via Redis Stack

Add SQL Server instead of Fake DB

Distributed caching using AddStackExchangeRedisCache

Angular / React frontend

Clean Architecture version

If you want any of these, I can generate the full code.

👨‍💻 Author

Mahesh Prakash
.NET Developer | Redis | Microservices | Cloud | Docker
Built with ❤️ using .NET 9, Redis, and Docker.

📄 License

This project is open source under the MIT License.
