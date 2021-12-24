using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MulitpleDb.Sample.Constants;
using MulitpleDb.Sample.Models;
using System;
using System.Threading.Tasks;

namespace MulitpleDb.Sample.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemaNames.MixSchemaNames)]
    [Route("api/[controller]")]
    [ApiController]
    public class RocketsController : ControllerBase
    {
        [HttpGet("{rocket}")]
        public async Task<String> Get(
            [FromRoute]String rocket,
            [FromQuery]RocketQueryModel query)
        {
            return await Task<String>.FromResult(
                $"Rocket {rocket} launched to {query.Planet} using {Enum.GetName(typeof(FuelTypeEnum), query.FuelType)} fuel type"
                );
        }
    }
}
