using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MulitpleDb.Sample.Data
{
    public class PlanetPairsConfiguration : IEntityTypeConfiguration<PlanetPair>
    {
        readonly IHostingEnvironment _environment;
        public PlanetPairsConfiguration(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public void Configure(EntityTypeBuilder<PlanetPair> builder)
        {
            builder.ToView("Planets", "Database2.dbo");

            if (_environment.EnvironmentName.Equals("XUnit", System.StringComparison.InvariantCulture))
                builder.HasKey(k => new { k.Id, k.Name, k.Pair });
            else
                builder.HasNoKey();
        }
    }
}
