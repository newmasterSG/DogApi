using Dogs.Domain.Entity;
using Dogs.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dogs.Infrastructure.Repository
{
    public class DogRepository : IDogRepository
    {
        private readonly DbContext _dbContext;

        public DogRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(DogEntity entity)
        {
            await _dbContext.AddAsync(entity);
        }

        public void Delete(DogEntity entity)
        {
            _dbContext.Remove(entity);
        }

        public async Task<ICollection<DogEntity>> GetAllAsync()
        {
            return await _dbContext.Set<DogEntity>().ToListAsync();
        }

        public async Task<DogEntity> GetByName(string name)
        {
            return await _dbContext.Set<DogEntity>().
                Where(item => item.Name.Equals(name)).
                FirstOrDefaultAsync();
        }

        public async Task<DogEntity> GetEntityAsync(int id)
        {
            return await _dbContext.Set<DogEntity>().FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<DogEntity> NoTracingWithName(string name)
        {
            return await _dbContext.Set<DogEntity>().
                AsNoTracking().
                Where(item => item.Name.Equals(name)).
                FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<DogEntity>> TakeAsync(int skipElements, 
            int takeElements, 
            (Expression<Func<DogEntity, object>> expression, bool ascending) sortOrder)
        {
            var query = _dbContext.Set<DogEntity>().AsNoTracking();

            if (sortOrder.ascending)
            {
                query = query.OrderBy(sortOrder.expression);
            }
            else
            {
                query = query.OrderByDescending(sortOrder.expression);
            }

            return query.Skip(skipElements)
                .Take(takeElements)
                .AsEnumerable();
        }

        public void Update(DogEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
