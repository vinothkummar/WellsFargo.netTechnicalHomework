using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wells.Fargo.Domain.Model;

namespace Wells.Fargo.Application.RuleEngine.Interface
{
    public interface IOMSRuleEngineProcessor
    {
        Task<Tuple<bool, string, string>> PortfolioTransactionProcessToOMSOutput(IEnumerable<Transaction> transactions, IEnumerable<Securities> Securities, IEnumerable<Portfolio> Portfolios, string folderPath);

    }
}
