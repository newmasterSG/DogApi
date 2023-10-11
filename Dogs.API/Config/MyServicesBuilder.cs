using AspNetCoreRateLimit;
using Dogs.Application.Interfaces;
using Dogs.Application.Services;
using Dogs.Infrastructure.Context;
using Dogs.Infrastructure.Interfaces;
using Dogs.Infrastructure.Repository;
using Dogs.Infrastructure.UnitOfWork;
using Microsoft.Extensions.Caching.Distributed;

namespace Dogs.API.Config
{
    public static class MyServicesBuilder
    {
        public static IServiceCollection AddingOwnDI(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IRepositoryFactory, RepositoryFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork<DogsContext>>();
            services.AddScoped<IDogService, DogService>();
            services.RateLimits();

            return services;
        }

        public static IServiceCollection RateLimits(this IServiceCollection services)
        {
            services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = 429;
                options.RealIpHeader = "X-Real-IP";
                options.ClientIdHeader = "X-ClientId";
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Period = "10s",
                        Limit = 10
                    }
                };
            });
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddInMemoryRateLimiting();

            return services;
        }
    }
}
