using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MulitpleDb.Sample.Data
{
    public class Database1Context : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;
        public Database1Context(
            DbContextOptions<Database1Context> options,
            ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }

        public DbSet<Planet> Planets { get; set; }
        public DbSet<PlanetPair> PlanetPairs { get; set; }
    }
}
