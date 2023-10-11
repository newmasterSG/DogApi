using Dogs.Domain.Entity;
using Dogs.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dogs.Infrastructure.Repository
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly Dictionary<Type, Func<DbContext, object>> _repositoryFactories;

        public RepositoryFactory()
        {
            _repositoryFactories = new Dictionary<Type, Func<DbContext, object>>();
        }

        public void Register<TEntity>(Func<DbContext, IRepository<TEntity>> factory) where TEntity : class
        {
            _repositoryFactories[typeof(TEntity)] = context => factory(context);
        }

        public void RegisterAllRepositories(DbContext context)
        {
            var repositoryTypes = Assembly.GetExecutingAssembly().GetTypes()
                                .Where(type => type.IsClass && !type.IsAbstract && type.GetInterfaces()
                                .Any(interfaceType =>
                                                        interfaceType.IsGenericType &&
                                                        interfaceType.GetGenericTypeDefinition() == typeof(IRepository<>)))
                                                        .ToList();

            foreach (var repositoryType in repositoryTypes)
            {
                var entityType = repositoryType.GetInterfaces()
                                .First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IRepository<>))
                                .GetGenericArguments()[0];
                var factory = BuildRepositoryFactory(repositoryType);
                _repositoryFactories[entityType] = context => factory(context);
            }
        }

        private Func<DbContext, object> BuildRepositoryFactory(Type repositoryType)
        {
            var dbContextType = typeof(DbContext);
            var constructorInfo = repositoryType.GetConstructors().FirstOrDefault();
            var dbContextParameter = constructorInfo.GetParameters()
                .FirstOrDefault(parameter => parameter.ParameterType == dbContextType);

            if (dbContextParameter != null)
            {
                return context =>
                {
                    var repository = Activator.CreateInstance(repositoryType, context);
                    return repository;
                };
            }

            throw new ArgumentException($"No suitable constructor found for {repositoryType.Name}");
        }

        public IRepository<TEntity> Create<TEntity>(DbContext context) where TEntity : class
        {
            if (_repositoryFactories.TryGetValue(typeof(TEntity), out var factory))
            {
                return (IRepository<TEntity>)factory(context);
            }

            throw new ArgumentException($"No repository registered for {typeof(TEntity).Name}");
        }
    }
}
