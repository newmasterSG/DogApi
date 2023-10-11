using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dogs.Infrastructure.Interfaces
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> Create<TEntity>(DbContext context) where TEntity : class;

        void RegisterAllRepositories(DbContext context);
    }
}
