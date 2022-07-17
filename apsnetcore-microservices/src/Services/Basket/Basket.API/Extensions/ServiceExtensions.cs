using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services) =>
            services.AddScoped<IBasketRepository, BasketRepository>()
                .AddTransient<ISerializeService, SerializeService>()
            ;
        
        public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetSection("CacheSettings:ConnectionString").Value;
            if(string.IsNullOrEmpty(redisConnectionString))
            {
                throw new ArgumentException("Redis Connection string is not configured."); // ArgumentException sử dụng khi tham số không thỏa mãn yêu cầu.
            }

            // Redis Configuration
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
            });
        }
    }
}
