using System;
using System.Collections.Generic;
using System.Text;

namespace Wells.Fargo.Domain.Model
{
    public class Securities
    {
        [Column(Name = "SecurityId", DataType = typeof(string))]
        public string SecurityId { get; set; }        

        [Column(Name = "ISIN", DataType = typeof(string))]
        public string ISIN { get; set; }

        [Column(Name = "Ticker", DataType = typeof(string))]
        public string Ticker { get; set; }

        [Column(Name = "CUSIP", DataType = typeof(string))]
        public string CUSIP { get; set; }
    }
}
