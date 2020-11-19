using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MulitpleDb.Sample.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MulitpleDb.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlanetsController : ControllerBase
    {
        
        readonly ILogger<PlanetsController> _logger;
        readonly Database1Context _database1Context;
        public PlanetsController(
            Database1Context database1Context,
            ILogger<PlanetsController> logger)
        {
            _database1Context = database1Context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<String>> Get()
        {
            return await _database1Context.Planets.AsNoTracking().Select(p=>p.Name).ToArrayAsync();
        }
    }
}
