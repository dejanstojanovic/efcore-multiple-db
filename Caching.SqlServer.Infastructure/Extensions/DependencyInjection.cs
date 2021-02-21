using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore;
namespace Caching.SqlServer.Infastructure.Extensions
{
    public static class DependencyInjection
    {
        public static void AddSqlServerCachingInfrastructure(this IServiceCollection services, String connectionString)
        {
            services.AddDbContext<CacheDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }

        public static void AddSqlServerCachingInfrastructure(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<CacheDbContext>(options);
        }

        public static void SetupSqlServerCachingInfrastructure(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<CacheDbContext>())
                {
                    context.Database.EnsureCreated();
                    context.Database.Migrate();
                }
            }
        }
    }
}
