using System;
using System.Collections.Generic;
using System.Text;

namespace Wells.Fargo.Domain.Model
{
    public class Transaction
    {
        [Column(Name = "SecurityId", DataType = typeof(string))]
        public string SecurityId { get; set; }

        [Column(Name = "PortfolioId", DataType = typeof(string))]
        public string PortfolioId { get; set; }

        [Column(Name = "Nominal", DataType = typeof(string))]
        public string Nominal { get; set; }

        [Column(Name = "OMS", DataType = typeof(string))]
        public string OMS { get; set; }

        [Column(Name = "TransactionType", DataType = typeof(string))]
        public string TransactionType { get; set; }
    }
}
