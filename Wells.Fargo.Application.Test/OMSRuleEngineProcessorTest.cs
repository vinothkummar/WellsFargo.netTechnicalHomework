using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Wells.Fargo.Application.RuleEngine.Interface;
using Wells.Fargo.Application.RuleEngine.Rules;
using Wells.Fargo.Domain.Model;
using Xunit;

namespace Wells.Fargo.Application.Test
{
    public class OMSRuleEngineProcessorTest
    {
        private readonly IOMSRuleEngineProcessor _ruleEngineProcessor;
        private readonly string _fileFolderRootPath;

        public OMSRuleEngineProcessorTest()
        {
            IServiceProvider services = ServiceInjector.ConfigureServices();

            _ruleEngineProcessor = services.GetRequiredService<IOMSRuleEngineProcessor>();

            string workingDirectory = Environment.CurrentDirectory;

            _fileFolderRootPath = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName;
        }

        [Fact]
        public void should_return_expected_oms_type_aaa_fields()
        {
            //Arrange
            var expectedFields = new { ISIN = "ISIN11111111", PortfolioCode = "p1", Nominal = "10", TransactionType = "BUY" };                      

            var omsTypeAAA = new OMSTypeAAA();

            //Act
            var response = omsTypeAAA.PortfolioTransactionProcessDataToOMS(GetTestData().Item1, GetTestData().Item2, GetTestData().Item3);


            //Assert
            response.Should().NotBeEmpty()
                             .And.HaveCount(x => x == 3);
            

            var y = response.ElementAt(0);

            Assert.True(expectedFields.ISIN == y.GetType().GetProperty("ISIN").GetValue(y, null) &&
                        expectedFields.PortfolioCode == y.GetType().GetProperty("PortfolioCode").GetValue(y, null) &&
                        expectedFields.Nominal == y.GetType().GetProperty("Nominal").GetValue(y, null) &&
                        expectedFields.TransactionType == y.GetType().GetProperty("TransactionType").GetValue(y, null));
        }

        [Fact]
        public void should_return_expected_oms_type_bbb_fields()
        {
            //Arrange
            var expectedFields = new { CUSIP = "CUSIP0001", PortfolioCode = "p1", Nominal = "10", TransactionType = "BUY" };

            var omsTypeBBB = new OMSTypeBBB();

            //Act
            var response = omsTypeBBB.PortfolioTransactionProcessDataToOMS(GetTestData().Item1, GetTestData().Item2, GetTestData().Item3);


            //Assert
            response.Should().NotBeEmpty()
                             .And.HaveCount(x => x == 3);
          

            var y = response.ElementAt(0);

            Assert.True(expectedFields.CUSIP == y.GetType().GetProperty("CUSIP").GetValue(y, null) &&
                        expectedFields.PortfolioCode == y.GetType().GetProperty("PortfolioCode").GetValue(y, null) &&
                        expectedFields.Nominal == y.GetType().GetProperty("Nominal").GetValue(y, null) &&
                        expectedFields.TransactionType == y.GetType().GetProperty("TransactionType").GetValue(y, null));
        }

        [Fact]
        public void should_return_expected_oms_type_ccc_fields()
        {
            //Arrange
            var expectedFields = new { PortfolioCode = "p1", Ticker="s1", Nominal = "10", TransactionType = "BUY" };

            var omsTypeCCC = new OMSTypeCCC();

            //Act
            var response = omsTypeCCC.PortfolioTransactionProcessDataToOMS(GetTestData().Item1, GetTestData().Item2, GetTestData().Item3);


            //Assert
            response.Should().NotBeEmpty()
                             .And.HaveCount(x => x == 3);
            

            var y = response.ElementAt(0);

            Assert.True(expectedFields.PortfolioCode == y.GetType().GetProperty("PortfolioCode").GetValue(y, null) &&
                        expectedFields.Ticker == y.GetType().GetProperty("Ticker").GetValue(y, null) &&
                        expectedFields.Nominal == y.GetType().GetProperty("Nominal").GetValue(y, null) &&
                        expectedFields.TransactionType == y.GetType().GetProperty("TransactionType").GetValue(y, null));
        }

        [Fact]
        public void should_generate_successfully_all_the_omstype_file_type_aaa_and_bbb_and_ccc()
        {
            //Arrange
            var folderPath = _fileFolderRootPath + @"\Wells.Fargo.API\Files";            

            //Act
            var response = _ruleEngineProcessor.PortfolioTransactionProcessToOMSOutput(GetTestData().Item1, GetTestData().Item2, GetTestData().Item3, folderPath).Result;


            //Assert
            Assert.True(response.Item1, "because OMS Processor generates the output file successfuly");

        }

        [Fact]
        public void should_return_omstype_file_aaa_with_comma_delimiter()
        {
            //Arrange
            var folderPath = _fileFolderRootPath + @"\Wells.Fargo.API\Files";

            var omsTypeAAA = new OMSTypeAAA();

            //Act
            var response = omsTypeAAA.PortfolioTransactionProcessToOMSOutput(GetTestData().Item1, GetTestData().Item2, GetTestData().Item3, folderPath).Result;


            //Assert
            response.Item3.Should().Be(",", "because it matches the same delimiter");
            response.Item3.Should().NotBe("|");

        }

        [Fact]
        public void should_return_omstype_file_bbb_with_pipe_delimiter()
        {
            //Arrange
            var folderPath = _fileFolderRootPath + @"\Wells.Fargo.API\Files";

            var omsTypeBBB = new OMSTypeBBB();

            //Act
            var response = omsTypeBBB.PortfolioTransactionProcessToOMSOutput(GetTestData().Item1, GetTestData().Item2, GetTestData().Item3, folderPath).Result;


            //Assert
            response.Item3.Should().Be("|", "because it matches the same delimiter");
            response.Item3.Should().NotBe(",");

        }

        [Fact]
        public void should_generate_omstype_file_with_extension_aaa()
        {
            //Arrange
            var folderPath = _fileFolderRootPath + @"\Wells.Fargo.API\Files";

            var omsTypeAAA = new OMSTypeAAA();

            //Act
            var response = omsTypeAAA.PortfolioTransactionProcessToOMSOutput(GetTestData().Item1, GetTestData().Item2, GetTestData().Item3, folderPath).Result;
            var expected = response.Item2.Split('.')[1];


            //Assert
            Assert.True(expected.Equals("aaa"));

        }

        [Fact]
        public void should_generate_omstype_file_with_extension_bbb()
        {
            //Arrange
            var folderPath = _fileFolderRootPath + @"\Wells.Fargo.API\Files";

            var omsTypeBBB = new OMSTypeBBB();

            //Act
            var response = omsTypeBBB.PortfolioTransactionProcessToOMSOutput(GetTestData().Item1, GetTestData().Item2, GetTestData().Item3, folderPath).Result;
            var expected = response.Item2.Split('.')[1];


            //Assert
            Assert.True(expected.Equals("bbb"));

        }

        [Fact]
        public void should_generate_omstype_file_with_extension_ccc()
        {
            //Arrange
            var folderPath = _fileFolderRootPath + @"\Wells.Fargo.API\Files";

            var omsTypeCCC = new OMSTypeCCC();

            //Act
            var response = omsTypeCCC.PortfolioTransactionProcessToOMSOutput(GetTestData().Item1, GetTestData().Item2, GetTestData().Item3, folderPath).Result;
            var expected = response.Item2.Split('.')[1];


            //Assert
            Assert.True(expected.Equals("ccc"));

        }



        private Tuple<IList<Transaction>, IList<Securities>, IList<Portfolio>> GetTestData()
        {
            IList<Transaction> transactions = new List<Transaction>{
                new Transaction { SecurityId="SecurityId",PortfolioId="PortfolioId",Nominal="Nominal",OMS="OMS",TransactionType="TransactionType"},
                new Transaction { SecurityId="1",PortfolioId="1",Nominal="10",OMS="AAA",TransactionType="BUY"},
                new Transaction { SecurityId="2",PortfolioId="2",Nominal="20",OMS="BBB",TransactionType="SELL"},
                new Transaction { SecurityId="1",PortfolioId="2",Nominal="30",OMS="CCC",TransactionType="BUY"}};

            IList<Securities> securities = new List<Securities>{
                new Securities {SecurityId="SecurityId",ISIN="ISIN",Ticker="Ticker",CUSIP="CUSIP"},
                new Securities {SecurityId="1",ISIN="ISIN11111111",Ticker="s1",CUSIP="CUSIP0001"},
                new Securities {SecurityId="2",ISIN="ISIN22222222",Ticker="s2",CUSIP="CUSIP0002"}
            };
            
            IList<Portfolio> portfolios = new List<Portfolio>{ 
                new Portfolio {PortfolioId="PortfolioId",PortfolioCode="PortfolioCode"},
                new Portfolio {PortfolioId="1",PortfolioCode="p1"},
                new Portfolio {PortfolioId="2",PortfolioCode="p2"}
            };

            return new Tuple<IList<Transaction>,IList<Securities>, IList<Portfolio>>(transactions,securities, portfolios);
        }
    }


}
