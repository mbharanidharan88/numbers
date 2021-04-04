using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Numbers.Models.DTO;
using Numbers.Worker;

namespace Numbers.Web.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BatchController : ControllerBase
    {
        private readonly ILogger<BatchController> _logger;
        private readonly IProcessor _processor;
        private readonly ChannelWriter<GeneratorBatch> _channel;
        

        public IServiceProvider Services { get; }

        public BatchController(ILogger<BatchController> logger, 
            IProcessor processor,
            IServiceProvider services,
            ChannelWriter<GeneratorBatch> channel)
        {
            _logger = logger;
            _processor = processor;
            Services = services;
            _channel = channel;
        }

        [HttpGet]
        [ActionName("StartBatch")]
        public async Task<JsonResult> StartBatch([FromQuery] BatchRequest request)
        {
            _processor.StartProcess(request);

            return new JsonResult(request.BatchId);
        }

        [HttpGet]
        [ActionName("Poll/{sessionId}")]
        public async Task<JsonResult> Poll(string sessionId)
        {
            var response = await _processor.GetSessionData(sessionId);

            return new JsonResult(response);
        }

        //[HttpGet("status")]
        //public async Task<JsonResult> Get()
        //{
        //    return new JsonResult(_processor.CurrentStatus);
        //}

        [HttpGet]
        [ActionName("Clear")]
        public async Task<JsonResult> Clear()
        {
            var isCleared = await _processor.Clear();

            return new JsonResult(isCleared);
        }

        [HttpGet]
        [ActionName("LastBatch")]
        public async Task<JsonResult> GetLastBatchDetails()
        {
            var response = await _processor.GetLastBatchDetails();

            return new JsonResult(response);
        }
    }
}