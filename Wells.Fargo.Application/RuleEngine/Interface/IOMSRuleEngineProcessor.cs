using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wells.Fargo.Domain.Model;

namespace Wells.Fargo.Application.RuleEngine.Interface
{
    public interface IOMSRuleEngineProcessor
    {
        Task<Tuple<bool, string, string>> PortfolioTransactionProcessToOMSOutput(IList<Transaction> transactions, IList<Securities> Securities, IList<Portfolio> Portfolios, string folderPath);

    }
}
