using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Caching.SqlServer.Infastructure
{
    public class CacheDbContextFactory : IDesignTimeDbContextFactory<CacheDbContext>
    {
        public CacheDbContext CreateDbContext(string[] args)
        {
            var dbContext = new CacheDbContext(new DbContextOptionsBuilder<CacheDbContext>().UseSqlServer(
                 new ConfigurationBuilder()
                     .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), $"appsettings.json"))
                     .Build()
                     .GetConnectionString("CacheDbConnection")
                 ).Options);

            return dbContext;
        }
    }
}
