using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dogs.Infrastructure.Interfaces
{
    public interface IRepository<TEntity> 
        where TEntity : class
    {
        Task AddAsync(TEntity entity);
        void Delete(TEntity entity);

        Task<TEntity> GetEntityAsync(int id);

        void Update(TEntity entity);

        Task<ICollection<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> TakeAsync(int skipElements, int takeElements);
    }
}
