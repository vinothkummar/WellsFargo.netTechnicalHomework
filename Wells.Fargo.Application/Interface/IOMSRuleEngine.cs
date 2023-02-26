using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wells.Fargo.Domain.Model;

namespace Wells.Fargo.Application.Interface
{
    public interface IOMSRuleEngine
    {
        Task<bool> OMSPortfolioTransactionProcessExport(string folderPath);

        Task<List<Transaction>> GetPortfolioTransactions(string folderPath);

        Task<List<Portfolio>> GetPortfolios(string folderPath);

        Task<List<Securities>> GetSecurities(string folderPath);
    }
}
