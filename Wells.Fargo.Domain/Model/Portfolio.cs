using System;
using System.Collections.Generic;
using System.Text;

namespace Wells.Fargo.Domain.Model
{
    public class Portfolio
    {
        [Column(Name = "PortfolioId", DataType = typeof(string))]
        public string PortfolioId { get; set; }        

        [Column(Name = "PortfolioCode", DataType = typeof(string))]
        public string PortfolioCode { get; set; }
    }
}
