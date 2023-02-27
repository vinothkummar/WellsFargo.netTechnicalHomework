using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wells.Fargo.Application.RuleEngine.Interface;
using Wells.Fargo.Domain.Model;

namespace Wells.Fargo.Application.RuleEngine.Rules
{
    public class OMSTypeAAA : IOMSRuleEngineProcessor
    {
        public Task<Tuple<bool, string, string>> PortfolioTransactionProcessToOMSOutput(IEnumerable<Transaction> transactions, IEnumerable<Securities> Securities, IEnumerable<Portfolio> Portfolios, string folderPath)
        {
            try
            {                
                var _csvFileOutput = PortfolioTransactionProcessDataToOMS(transactions, Securities, Portfolios);

                var _fileName = "OMSOutput.aaa";

                var _delimiter = ",";

                using (var textWriter = new StreamWriter(folderPath + "\\" + _fileName))
                {                  

                    var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = _delimiter };
                    
                    var writer = new CsvWriter(textWriter, config);

                    writer.WriteRecords(_csvFileOutput);
                }

                return Task.FromResult(new Tuple<bool,string,string>(true, _fileName, _delimiter));
            }


            catch (Exception ex)
            {

                throw ex;
            }
        }


        public Func<IEnumerable<Transaction>, IEnumerable<Securities>, IEnumerable<Portfolio>, IEnumerable<dynamic>> PortfolioTransactionProcessDataToOMS = (IEnumerable<Transaction> transactions,IEnumerable<Securities> Securities, IEnumerable<Portfolio> Portfolios) =>
                         transactions.Join(Securities, t =>t.SecurityId, s => s.SecurityId, (t, s) => new { t, s })
                                     .Join(Portfolios, ts =>ts.t.PortfolioId, p => p.PortfolioId, (ts, p) => new { ts, p })
                                     .Select(aaa => new
                                     {
                                         aaa.ts.s.ISIN,
                                         aaa.p.PortfolioCode,
                                         aaa.ts.t.Nominal,
                                         aaa.ts.t.TransactionType
                                     }).Skip(1);
    }
}
