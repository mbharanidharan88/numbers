using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Numbers.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MultiplierController : ControllerBase
    {
        private readonly ILogger<MultiplierController> _logger;

        public MultiplierController(ILogger<MultiplierController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{multiplicand}")]
        public async Task<int> Get(int multiplicand)
        {
            await Task.Delay(GetRandomDelay());

            return MultiplyNumber(multiplicand);
        }

        private int GetRandomDelay()
        {
            Random rnd = new Random();

            return rnd.Next(5, 10) * 1000;
        }

        private int MultiplyNumber(int multiplicand)
        {
            return multiplicand * GetMultiplier();
        }

        private int GetMultiplier()
        {
            Random rnd = new Random();

            return rnd.Next(2, 4);
        }
    }
}