using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using MulitpleDb.Sample.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MulitpleDb.Sample.Tests
{

    public class DataTests
    {
        [Fact]
        public async Task PlanetPairs_Should_Return_Pairs()
        {
            //Arrange
            var loggerFactory = new Mock<ILoggerFactory>();
            var options = new DbContextOptionsBuilder<Database1Context>()
                 .UseInMemoryDatabase("InMemoryDb")
                 .Options;
            var _dbContext = new Database1Context(options,loggerFactory.Object);
            await _dbContext.PlanetPairs.AddRangeAsync(
                new PlanetPair() { Id = 1, Name = "Earth", Pair = "Earth - Mars" },
                new PlanetPair() { Id = 2, Name = "Saturn", Pair = "Saturn - Neptune" },
                new PlanetPair() { Id = 3, Name = "Saturn", Pair = "Saturn - Venus" }
                );
            await _dbContext.SaveChangesAsync();

            //Act
            var result = await _dbContext.PlanetPairs.Select(p => p).ToArrayAsync();

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
