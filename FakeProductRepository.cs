namespace RedisDemoAPI
{
    public class FakeProductRepository
    {
        public async Task<decimal> GetPriceFromDbAsync(int productId)
        {
            await Task.Delay(3000); // simulate slow DB
            return 100 + productId;
        }
    }
}
