using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Numbers.Models.DTO;
using Numbers.Models.Enums;
using Numbers.Models.Handlers;
using System;
using System.Collections.Concurrent;
using Numbers.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Numbers.Database;
using Numbers.Models.DbModels;
using Microsoft.VisualBasic.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Net.NetworkInformation;

namespace Numbers.Worker
{
    public class Processor : IProcessor
    {
        public ConcurrentDictionary<int, GeneratorBatch> BatchDetails { get; set; }
        public BatchRequest BatchRequest { get; set; }
        public BatchStatus CurrentStatus { get; set; }

        private readonly ILogger<Processor> _logger;
        private readonly ChannelWriter<GeneratorBatch> _channel;
        private readonly GeneratorManager _generatorManager;

        private readonly ChannelWriter<MultiplierBatch> _multiplierChannel;
        private readonly MultiplierManager _multiplierManager;
        private readonly IServiceScopeFactory _scopeFactory;
        private IBatchRepository _batchRepository;
        private readonly IBatchRetreivalService _batchRetreivalService;

        private int _numbersPerBatch;

        

        public Processor(ILogger<Processor> logger,
                            ChannelWriter<GeneratorBatch> generatorChannel,
                            ChannelWriter<MultiplierBatch> multiplierChannel,
                            GeneratorManager generatorManager,
                            MultiplierManager multiplierManager,
                            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _channel = generatorChannel;
            _multiplierChannel = multiplierChannel;
            _generatorManager = generatorManager;
            _multiplierManager = multiplierManager;
            _scopeFactory = scopeFactory;

            BatchDetails = new ConcurrentDictionary<int, GeneratorBatch>();

            RegisterEvents();
        }

        #region Public Mthods

        /// <summary>
        /// To Start New Batch
        /// </summary>
        /// <param name="batchDetail"></param>
        /// <returns cref={Task<string>}></returns>
        public async Task<string> StartProcess(BatchRequest batchDetail)
        {
            UpdateCurrentStatus(BatchStatus.Started);

            BatchDetails = new ConcurrentDictionary<int, GeneratorBatch>();
            _numbersPerBatch = batchDetail.NumbersPerBatch;

            for (var i = 0; i < batchDetail.NumberOfBatches; i++)
            {
                var batch = new GeneratorBatch { BatchNumber = i + 1, NumbersPerBatch = _numbersPerBatch };

                BatchDetails.TryAdd(i + 1, batch);

                await _channel.WriteAsync(batch);
            }

            return batchDetail.BatchId;
        }

        /// <summary>
        /// To get the polling data
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns cref={Task<BatchResponse>}></returns>
        public async Task<BatchResponse> GetSessionData(string batchId)
        {
            var list = new List<GeneratorBatch>();
            var isProcessCompleted = false;
                        
            foreach (KeyValuePair<int, GeneratorBatch> item in BatchDetails)
            {
                var generatorBatch = item.Value;

                generatorBatch.BatchId = batchId;
                isProcessCompleted = generatorBatch.MultipliedNumbers.Count == _numbersPerBatch;

                list.Add(generatorBatch);
            }

            if (isProcessCompleted)
            {
                UpdateCurrentStatus(BatchStatus.Completed);
            }

            return new BatchResponse
            {
                
                CurrentStatus = CurrentStatus,
                ResponseData = list
            };
        }

        public async Task<bool> Clear()
        {
            var isCleared = false;

            try
            {
                var bds = new List<BatchDetail>();

                foreach (KeyValuePair<int, GeneratorBatch> item in BatchDetails)
                {
                    var generatedBatch = item.Value;

                    var bd = new BatchDetail
                    {
                        BatchId = generatedBatch.BatchId,
                        NumberOfBatches = generatedBatch.GeneratedNumbers.Count(),
                        NumberPerBatches = generatedBatch.NumbersPerBatch,
                        BatchTotal = generatedBatch.Total,
                        GeneratedNumbers = generatedBatch.GeneratedNumbers.Select(x => new BatchGeneratedNumber { GeneratedNumber = x }).ToList(),
                        MultipliedNumbers = generatedBatch.MultipliedNumbers.Select(x => new BatchMultipliedNumber { MultipliedNumber = x }).ToList()
                    };

                    bds.Add(bd);
                }

                var list = new List<BatchDetail>();

                using (var scope = _scopeFactory.CreateScope())
                {

                    _batchRepository = scope.ServiceProvider.GetRequiredService<IBatchRepository>();
                    await _batchRepository.AddRangeAsync(bds);
                    await _batchRepository.SaveChangesAsync();

                    var aaa = await _batchRepository.BatchDetails;
                }

                isCleared = true;
                BatchDetails = new ConcurrentDictionary<int, GeneratorBatch>();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Clear: Processor: An error occured");
            }

            return isCleared;
        }

        public async Task<IEnumerable<BatchDetail>> GetLastBatchDetails()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    _batchRepository = scope.ServiceProvider.GetRequiredService<IBatchRepository>();


                    return await _batchRepository.BatchDetails;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetLastBatchDetails: Processor: An error occured");
            }

            return default;
        }

        #endregion 

        #region Events

        private void OnGeneratedNumber(object source, GenerateBatchEventArgs args)
        {
            UpdateCurrentStatus(BatchStatus.GeneratingNumbers);

            _logger.LogInformation("Processor: OnGeneratedNumber");

            var batchNumber = args.BatchNumber;
            var generatedNumber = args.GeneratedNumber;

            UpdateGeneratedNumber(batchNumber, generatedNumber);

        }

        private void OnMulipliedNumber(object source, MultiplyBatchEventArgs args)
        {
            UpdateCurrentStatus(BatchStatus.MultiplyingNumbers);

            _logger.LogInformation("Processor: OnMulipliedNumber");

            var batchNumber = args.BatchNumber;
            var multipliedNumber = args.MultipliedNumber;

            UpdateMulitpliedNumber(batchNumber, multipliedNumber);
        }

        #endregion

        private async Task UpdateGeneratedNumber(int batchNumber, int generatedNumber)
        {
            //BatchResponse.CurrentStatus = BatchStatus.GeneratingNumbers;

            BatchDetails[batchNumber].GeneratedNumbers.Add(generatedNumber);

            await _multiplierChannel.WriteAsync(new MultiplierBatch { BatchNumber = batchNumber, NumberToMultiply = generatedNumber });
        }

        

        private void UpdateMulitpliedNumber(int batchNumber, int multipliedNumber)
        {
            //BatchResponse.CurrentStatus = BatchStatus.GeneratingNumbers;

            BatchDetails[batchNumber].MultipliedNumbers.Add(multipliedNumber);

            AggregateMultipliedNumbers();
        }

        private async Task AggregateMultipliedNumbers()
        {
            await Task.Run(() =>
             {
                 foreach (KeyValuePair<int, GeneratorBatch> item in BatchDetails)
                 {
                     var batch = item.Value;

                     batch.Total = batch.MultipliedNumbers.Aggregate(0, (a, b) => a + b);
                 }
             });
        }

        private void RegisterEvents()
        {
            _generatorManager.GeneratedNumber += OnGeneratedNumber;
            _multiplierManager.MultipliedNumber += OnMulipliedNumber;
        }

        private void UpdateCurrentStatus(BatchStatus status)
        {
            CurrentStatus = status;
        }

    }
}
