using Caching.SqlServer.Infastructure.Configurations;
using Caching.SqlServer.Infastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Caching.SqlServer.Infastructure
{
    public partial class CacheDbContext : DbContext
    {
        public CacheDbContext()
        {
        }

        public CacheDbContext(DbContextOptions<CacheDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cache> Cache { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CacheConfiguration());
        }
    }
}
