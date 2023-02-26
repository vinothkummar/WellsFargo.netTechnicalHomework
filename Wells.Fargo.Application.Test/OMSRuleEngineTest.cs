using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wells.Fargo.Application.Interface;
using Xunit;

namespace Wells.Fargo.Application.Test
{
    public class OMSRuleEngineTest
    {
         
        private readonly IOMSRuleEngine _ruleEngine;
        private readonly string _fileFolderRootPath;

        public OMSRuleEngineTest()
        {
            IServiceProvider services = ServiceInjector.ConfigureServices();

            _ruleEngine = services.GetRequiredService<IOMSRuleEngine>();

            string workingDirectory = Environment.CurrentDirectory;
            
            _fileFolderRootPath = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName;            
        }

        [Fact]
        public void should_return_portfolio_Transaction_processor_response_state_as_boolean()
        {
            
            var folderPath = _fileFolderRootPath+@"\Wells.Fargo.API\Files";                       
            
            var response = _ruleEngine.OMSPortfolioTransactionProcessExport(folderPath).Result;

            Assert.True(response.GetType() == typeof(bool));
        }

        [Fact]
        public void should_return_Transaction_data_from_csv()
        {
            var folderPath = _fileFolderRootPath + @"\Wells.Fargo.API\Files";

            var response = _ruleEngine.GetPortfolioTransactions(folderPath).Result;

            response.Should().HaveCount(4, "because csv file contains four items");
            response.Should().NotBeNull();
        }

        [Fact]
        public void should_return_metadata_securities_data_from_csv()
        {
            var folderPath = _fileFolderRootPath + @"\Wells.Fargo.API\Files";

            var response = _ruleEngine.GetSecurities(folderPath).Result;

            response.Should().HaveCount(3, "because csv file contains two items");
            response.Should().NotBeNull();

        }

        [Fact]
        public void should_return_metadata_portfolios_data_from_csv()
        {
            var folderPath = _fileFolderRootPath + @"\Wells.Fargo.API\Files";

            var response = _ruleEngine.GetPortfolios(folderPath).Result;

            response.Should().HaveCount(3, "because csv file contains two items");
            response.Should().NotBeNull();

        }
    }
}
