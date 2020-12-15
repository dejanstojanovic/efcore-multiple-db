using Caching.SqlServer.Infastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Caching.SqlServer.Infastructure.Configurations
{
    public class CacheConfiguration : IEntityTypeConfiguration<Cache>
    {
        public void Configure(EntityTypeBuilder<Cache> builder)
        {
            builder.ToTable(name:"Cache", schema: "app");

            builder.HasIndex(e => e.ExpiresAtTime);

            builder.Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(449);

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Value).IsRequired();
        }
    }
}
