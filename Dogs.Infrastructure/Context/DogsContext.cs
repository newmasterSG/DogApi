using Dogs.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Dogs.Infrastructure.Context
{
    public class DogsContext : DbContext
    {
        public DbSet<DogEntity> Dogs { get; set; }

        public DogsContext(DbContextOptions<DogsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
