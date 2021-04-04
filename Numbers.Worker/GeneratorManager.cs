using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Numbers.Models.DTO;
using Numbers.Models.Handlers;
using Numbers.Service.Interfaces;

namespace Numbers.Worker
{
    public class GeneratorManager : BackgroundService
    {
        private readonly ILogger<GeneratorManager> _logger;
        private readonly ChannelReader<GeneratorBatch> _channel;
        private readonly IGeneratorService _generatorService;

        public delegate void GeneratedNumberEventHandler(object source, GenerateBatchEventArgs e);
        public event GeneratedNumberEventHandler GeneratedNumber;

        public GeneratorManager(ILogger<GeneratorManager> logger, 
                                ChannelReader<GeneratorBatch> channelReader,
                                IGeneratorService generatorService)
        {
            _logger = logger;
            _channel = channelReader;
            _generatorService = generatorService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var item in _channel.ReadAllAsync(stoppingToken))
            {
                try
                {
                    _logger.LogInformation("GeneratorManager: Channel started reading at: {time}", DateTimeOffset.Now);

                    
                     GenerateNumber(item);
                     
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "An error occured");
                }
            }
        }

        protected virtual void OnGeneratedNumber(int batchNumber, int generatedNumber)
        {
            _logger.LogInformation($"GeneratorManager: OnGeneratedNumber {batchNumber} - {generatedNumber}");

            GeneratedNumber?.Invoke(this, new GenerateBatchEventArgs { BatchNumber = batchNumber, GeneratedNumber = generatedNumber});
        }

        private async Task GenerateNumber(Batch batch)
        {
            _logger.LogInformation("GeneratorManager: GenerateNumber");

            var batchNumber = batch.BatchNumber;

            for (var i = 0; i < batch.NumbersPerBatch; i++)
            {
                await Task.Run(async () =>
                 {
                     var num = await _generatorService.GenerateNumber();

                     _logger.LogInformation($"GeneratorManager: Generated Number -{num} at {DateTimeOffset.Now}");

                     OnGeneratedNumber(batchNumber, num);
                 });
            }
        }
    }
}
