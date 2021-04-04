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
    public class GeneratorController : ControllerBase
    {
        private readonly ILogger<GeneratorController> _logger;

        public GeneratorController(ILogger<GeneratorController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<int> Get()
        {
            await Task.Delay(GetRandomDelay());

            return GenerateRandomNumber();
        }

        [HttpGet("{info}")]
        public async Task<IActionResult> Get(string info)
        {
            return Ok("Numbers API Version 1.0");
        }

        private int GetRandomDelay()
        {
            Random rnd = new Random();

            return rnd.Next(5, 10) * 1000;
        }

        private int GenerateRandomNumber()
        {
            Random rnd = new Random();

            return rnd.Next(1, 100);
        }
    }
}