using Dogs.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dogs.Infrastructure.Context.Config
{
    public class DogConfig : IEntityTypeConfiguration<DogEntity>
    {
        public void Configure(EntityTypeBuilder<DogEntity> builder)
        {
            builder.ToTable("Dog");

            builder.HasKey(x => x.Id);

            builder.HasIndex(item => item.Name);
        }
    }
}
