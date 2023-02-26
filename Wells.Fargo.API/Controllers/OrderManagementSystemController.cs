using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wells.Fargo.Application;
using Wells.Fargo.Application.Interface;

namespace Wells.Fargo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderManagementSystemController : ControllerBase
    {
        private readonly ILogger<OrderManagementSystemController> _logger;
        private readonly IOMSRuleEngine _OMSRuleEngineProcessor;

        public OrderManagementSystemController(ILogger<OrderManagementSystemController> logger, IOMSRuleEngine OMSRuleEngineProcessor)
        {
            _logger = logger;
            _OMSRuleEngineProcessor = OMSRuleEngineProcessor;
        }

        [HttpGet("CreateOMSPortfolioOutputs")]
        public async Task GetCreatOMSOutput()
        {            
            try
            {
                const String folderName = "Files";
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                await _OMSRuleEngineProcessor.OMSPortfolioTransactionProcessExport(folderPath);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to generate the OMS output: {ex}");

            }


        }
    }
}
