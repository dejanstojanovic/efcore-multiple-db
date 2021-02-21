using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MulitpleDb.Sample.Data
{
    public class Database1Context : DbContext
    {
        #region Dynamic configuration load
        static readonly IEnumerable<(Type ConfigType, Type EntityType)> _configTypeInfos = typeof(Database1Context).Assembly.GetTypes()
                       .Where(c => c.GetInterfaces()
                           .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
                                   .Select(c => (
                                       ConfigType: c,
                                       EntityType:
                                       c.GetInterfaces()
                                           .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                                           ?.GetGenericArguments().FirstOrDefault()
                                   ))
                                   .Where(t => t.EntityType != null).ToArray();

        static readonly MethodInfo _applyGenericMethod = typeof(ModelBuilder).GetMethod("ApplyConfiguration", BindingFlags.Instance | BindingFlags.Public);
        #endregion

        readonly IHostingEnvironment _environment;

        private readonly ILoggerFactory _loggerFactory;
        public Database1Context(
            DbContextOptions<Database1Context> options,
            ILoggerFactory loggerFactory,
            IHostingEnvironment environment
            ) : base(options)
        {
            _loggerFactory = loggerFactory;
            _environment = environment;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

            foreach (var configTypeInfo in _configTypeInfos)
            {
                var hasCtorWithEnvironment = configTypeInfo.ConfigType.GetConstructor(new[] { typeof(IHostingEnvironment) }) != null;
                object config;

                if (hasCtorWithEnvironment)
                    config = Activator.CreateInstance(configTypeInfo.ConfigType, _environment);
                else
                    config = Activator.CreateInstance(configTypeInfo.ConfigType);

                var applyConcreteMethod = _applyGenericMethod.MakeGenericMethod(configTypeInfo.EntityType);
                applyConcreteMethod.Invoke(modelBuilder, new object[] { config });
            }
        }

        public DbSet<Planet> Planets { get; set; }
        public DbSet<PlanetPair> PlanetPairs { get; set; }
    }
}
