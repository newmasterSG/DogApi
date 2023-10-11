using Dogs.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dogs.Infrastructure.UnitOfWork
{
    public class UnitOfWork<TContext> : 
        IUnitOfWork where TContext : DbContext
    {
        private Dictionary<Type, object> _repositories;
        private IRepositoryFactory _repositoryFactory;

        private readonly TContext _context;

        public UnitOfWork(TContext context, IRepositoryFactory repositoryFactory)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));

            _repositoryFactory.RegisterAllRepositories(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public IRepository<TEntity> GetRepository<TEntity>() 
            where TEntity : class
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            var entityType = typeof(TEntity);

            if (!_repositories.ContainsKey(entityType))
            {
                _repositories[entityType] = _repositoryFactory.Create<TEntity>(_context);
            }

            return (IRepository<TEntity>)_repositories[entityType];
        }
    }
}
