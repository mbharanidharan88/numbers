using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Numbers.Models.DTO;
using Numbers.Models.Handlers;
using Numbers.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Numbers.Worker
{
    public class MultiplierManager : BackgroundService
    {
        private readonly ILogger<MultiplierManager> _logger;
        private readonly ChannelReader<MultiplierBatch> _channel;
        private readonly IMultiplierService _multiplierService;

        public delegate void MultipliedNumberEventHandler(object source, MultiplyBatchEventArgs e);
        public event MultipliedNumberEventHandler MultipliedNumber;

        public MultiplierManager(ILogger<MultiplierManager> logger,
                                ChannelReader<MultiplierBatch> channelReader,
                                IMultiplierService multiplierService)
        {
            _logger = logger;
            _channel = channelReader;
            _multiplierService = multiplierService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var item in _channel.ReadAllAsync(stoppingToken))
            {
                try
                {
                    _logger.LogInformation("MultiplierManager: Channel started reading at: {time}", DateTimeOffset.Now);

                    await MultiplyNumber(item);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "An error occured");
                }
            }
        }

        protected virtual void OnMultipliedNumber(int batchNumber, int multipliedNumber)
        {
            _logger.LogInformation($"MultiplierManager: OnMultipliedNumber {batchNumber} - {multipliedNumber}");

            MultipliedNumber?.Invoke(this, new MultiplyBatchEventArgs { BatchNumber = batchNumber, MultipliedNumber = multipliedNumber });
        }

        private async Task MultiplyNumber(MultiplierBatch batch)
        {
            _logger.LogInformation("MultiplierManager: MultiplyNumber");

            var batchNumber = batch.BatchNumber;
            var numberToMultiply = batch.NumberToMultiply;

            //Random rnd = new Random();
            //var num = rnd.Next(2, 4);

            //var multipliedNumber = numberToMultiply * num;

            //_logger.LogInformation($"MultiplierManager: Multiply Number {numberToMultiply} - {num} - {multipliedNumber}");

            //await Task.Delay(3000);

            await Task.Run(async () =>
            {
                var num = await _multiplierService.MultiplyNumber(numberToMultiply);

                _logger.LogInformation($"MultiplierManager: Multiplied Number -{num} at {DateTimeOffset.Now}");

                OnMultipliedNumber(batchNumber, num);
            });

            //OnMultipliedNumber(batchNumber, multipliedNumber);

        }
    }
}
