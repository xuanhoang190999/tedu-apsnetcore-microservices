using Infrastructure.Extensions;
using Inventory.Grpc.Repositories;
using Inventory.Grpc.Repositories.Interfaces;
using MongoDB.Driver;
using Shared.Configurations;

namespace Inventory.Product.API.Extensions
{
    public static class ServiceExtensions
    {
        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettings = configuration.GetSection(nameof(MongoDBSettings))
                .Get<MongoDBSettings>();
            services.AddSingleton(databaseSettings);

            return services;
        }

        private static string getMongoConnectionString(this IServiceCollection services)
        {
            var settings = services.GetOptions<MongoDBSettings>(nameof(MongoDBSettings));
            if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
                throw new ArgumentException("MongoDBSettings is not configured.");

            var databaseName = settings.DatabaseName;
            var mongodbConnectionString = settings.ConnectionString + "/" + databaseName + "?authSource=admin"; // authSource tùy dự án sẽ cần hoặc không, thêm vào cho chắc.

            return mongodbConnectionString;
        }

        public static void ConfigureMongoDbClient(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(
                new MongoClient(getMongoConnectionString(services)))
                .AddScoped(x => x.GetService<IMongoClient>()?.StartSession());
        }

        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IInventoryRepository, InventoryRepository>();
        }
    }
}
