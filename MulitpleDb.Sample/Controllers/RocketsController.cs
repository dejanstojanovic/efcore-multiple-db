using Microsoft.AspNetCore.Mvc;
using MulitpleDb.Sample.Models;
using System;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using MulitpleDb.Sample.Validators;
using System.ComponentModel.DataAnnotations;

namespace MulitpleDb.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RocketsController : ControllerBase
    {
        [HttpPost("{rocket}")]
        public async Task<String> Post(
            [FromRoute][CustomizeValidator(Interceptor = typeof(PlanetValidator), Skip = true)] String rocket,
            [FromQuery][Required] FuelTypeEnum fuelType,
            [FromQuery][Required] String planet)
        {
            return $"Rocket {rocket} launched to {planet} using {Enum.GetName(typeof(FuelTypeEnum), fuelType)} fuel type";
        }
    }
}
