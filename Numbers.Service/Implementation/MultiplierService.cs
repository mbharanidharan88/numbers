using Microsoft.Extensions.Logging;
using Numbers.Service.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Numbers.Service.Implementation
{
    public class MultiplierService : IMultiplierService
    {
        private readonly HttpClient _client;
        private readonly ILogger<MultiplierService> _logger;

        public MultiplierService(HttpClient httpClient, ILogger<MultiplierService> logger)
        {
            _client = httpClient;
            _logger = logger;
        }
        public async Task<int> MultiplyNumber(int multiplicand)
        {
            try
            {
                var response = await _client.GetAsync("multiplier/" + multiplicand);

                if (response.IsSuccessStatusCode)
                {
                    var number = await response.Content.ReadAsStringAsync();

                    return Convert.ToInt32(number);
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, "MultiplierService: An error occured");
            }

            return default;
        }
    }
}
