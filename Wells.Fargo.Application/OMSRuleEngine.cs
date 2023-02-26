using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wells.Fargo.Application.Interface;
using Wells.Fargo.Application.RuleEngine.Interface;
using Wells.Fargo.Domain.Model;

namespace Wells.Fargo.Application
{
    public class OMSRuleEngine : IOMSRuleEngine
    {        
        private IEnumerable<IOMSRuleEngineProcessor> _rules;

        public OMSRuleEngine()
        {
            var ruleType = typeof(IOMSRuleEngineProcessor);

            _rules = this.GetType().Assembly.GetTypes()
                .Where(p => ruleType.IsAssignableFrom(p) && !p.IsInterface)
                .Select(r => Activator.CreateInstance(r) as IOMSRuleEngineProcessor); 
        }
        
        public async Task<bool> OMSPortfolioTransactionProcessExport(string folderPath)
        {
            bool processorState = false;

            foreach (var rule in _rules)
            {
                var result = await rule.PortfolioTransactionProcessToOMSOutput(GetPortfolioTransactions(folderPath).Result,
                                                                                   GetSecurities(folderPath).Result,
                                                                                   GetPortfolios(folderPath).Result, folderPath);
                processorState = result.Item1;
            }

            return processorState;
        }

        public Task<List<Portfolio>> GetPortfolios(string folderPath)
        {
            string portfoliosFilePath = Path.Combine(folderPath, Path.GetFileName("Portfolios.csv"));
            var portfolios = new FileReader<Portfolio>(portfoliosFilePath, ",").ToList();
            return Task.FromResult(portfolios);
        }

        public Task<List<Securities>> GetSecurities(string folderPath)
        {
            string securitiesFilePath = Path.Combine(folderPath, Path.GetFileName("Securities.csv"));
            var secruities = new FileReader<Securities>(securitiesFilePath, ",").ToList();
            return Task.FromResult(secruities);
        }

        public Task<List<Transaction>> GetPortfolioTransactions(string folderPath)
        {
            string transactionFilePath = Path.Combine(folderPath, Path.GetFileName("Transactions.csv"));
            var transactions = new FileReader<Transaction>(transactionFilePath, ",").ToList();
            return Task.FromResult(transactions);
        }
    }
}
