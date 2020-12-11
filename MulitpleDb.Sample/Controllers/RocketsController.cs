using Microsoft.AspNetCore.Mvc;
using MulitpleDb.Sample.Models;
using System;
using System.Threading.Tasks;

namespace MulitpleDb.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RocketsController : ControllerBase
    {
        [HttpPost("{rocket}")]
        public async Task<String> Post(
            [FromRoute] String rocket,
            [FromQuery] FuelTypeEnum fuelType,
            [FromQuery] String planet)
        {
            return $"Rocket {rocket} launched to {planet} using {Enum.GetName(typeof(FuelTypeEnum), fuelType)} fuel type";
        }
    }
}
