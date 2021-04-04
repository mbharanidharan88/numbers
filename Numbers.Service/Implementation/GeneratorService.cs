using Microsoft.Extensions.Logging;
using Numbers.Service.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Numbers.Service.Implementation
{
    public class GeneratorService : IGeneratorService
    {
        private readonly HttpClient _client;
        private readonly ILogger<GeneratorService> _logger;

        public GeneratorService(HttpClient httpClient, ILogger<GeneratorService> logger)
        {
            _client = httpClient;
            _logger = logger;
        }
        public async Task<int> GenerateNumber()
        {
            try
            {
                var response = await _client.GetAsync("generator"); //.ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var number = await response.Content.ReadAsStringAsync();

                    return Convert.ToInt32(number);
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, "GeneratorService: An error occured");
            }

            return default;
        }
    }
}
