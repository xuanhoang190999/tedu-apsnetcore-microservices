using Inventory.Product.API.Persistence;
using MongoDB.Driver;

namespace Inventory.Product.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var settings = services.GetService<DatabaseSettings>();
            if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
                throw new ArgumentNullException("DatabaseSettings is not configured");

            var mongoClient = services.GetRequiredService<IMongoClient>();
            new IventoryDbSeed()
                .SeedDataAsync(mongoClient, settings)
                .Wait();
            return host;
        }
    }
}
