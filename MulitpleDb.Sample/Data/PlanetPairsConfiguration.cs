using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MulitpleDb.Sample.Data
{
    public class PlanetPairsConfiguration : IEntityTypeConfiguration<PlanetPair>
    {
        public void Configure(EntityTypeBuilder<PlanetPair> builder)
        {
            builder.ToView("Planets", "Database2.dbo");
            builder.HasNoKey();
        }
    }
}
