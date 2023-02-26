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
    public class OMSTypeCCC : IOMSRuleEngineProcessor
    {     

        public Task<Tuple<bool, string, string>> PortfolioTransactionProcessToOMSOutput(IList<Transaction> transactions, IList<Securities> Securities, IList<Portfolio> Portfolios, string folderPath)
        {
            try
            {
                //var csvFileOutput = transactions.Skip(1).Join(Securities.Skip(1), t => t.SecurityId, s => s.SecurityId, (t, s) => new { t, s })
                //                            .Join(Portfolios.Skip(1), ts => ts.t.PortfolioId, p => p.PortfolioId, (ts, p) => new { ts, p })
                //                            .Select(aaa => new
                //                            {
                //                                aaa.p.PortfolioCode,
                //                                aaa.ts.s.Ticker,
                //                                aaa.ts.t.Nominal,
                //                                aaa.ts.t.TransactionType
                //                            }).ToList();

                var csvFileOutput = PortfolioTransactionProcessDataToOMS(transactions, Securities, Portfolios);

                var _fileName = "OMSOutput.ccc";

                var _delimiter = ",";

                using (var textWriter = new StreamWriter(folderPath + "\\" + _fileName))
                {
                    var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = _delimiter, HasHeaderRecord = false };
                    var writer = new CsvWriter(textWriter, config);
                    writer.WriteRecords(csvFileOutput);
                }

                return Task.FromResult(new Tuple<bool, string, string>(true, _fileName, _delimiter));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Func<IList<Transaction>, IList<Securities>, IList<Portfolio>, IEnumerable<dynamic>> PortfolioTransactionProcessDataToOMS = (IList<Transaction> transactions, IList<Securities> Securities, IList<Portfolio> Portfolios) =>
                        transactions.Skip(1).Join(Securities.Skip(1), t => t.SecurityId, s => s.SecurityId, (t, s) => new { t, s })
                                    .Join(Portfolios.Skip(1), ts => ts.t.PortfolioId, p => p.PortfolioId, (ts, p) => new { ts, p })
                                     .Select(aaa => new
                                     {
                                         aaa.p.PortfolioCode,
                                         aaa.ts.s.Ticker,
                                         aaa.ts.t.Nominal,
                                         aaa.ts.t.TransactionType
                                     });


    }
}
