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
            services.AddSingleton<IRepositoryFactory, RepositoryFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork<DogsContext>>();
            services.AddScoped<IDogService, DogService>();

            return services;
        }
    }
}
