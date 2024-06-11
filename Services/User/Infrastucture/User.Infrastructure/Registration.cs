using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Interfaces.RedisCache;
using User.Infrastructure.Redis;

namespace User.Infrastructure
{
    public static class Registration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RedisCacheSettings>(configuration.GetSection("RedisCacheSettings"));

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisSettings = sp.GetRequiredService<IOptions<RedisCacheSettings>>().Value;
                var logger = sp.GetRequiredService<ILogger<ConnectionMultiplexer>>();

                var watch = System.Diagnostics.Stopwatch.StartNew();
                var multiplexer = ConnectionMultiplexer.Connect(redisSettings.ConnectionString);
                watch.Stop();

                logger.LogInformation($"Redis connection established in {watch.ElapsedMilliseconds} ms");
                return multiplexer;
            });

            services.AddTransient<IRedisCacheService, RedisCacheService>();

        }
    }
}
