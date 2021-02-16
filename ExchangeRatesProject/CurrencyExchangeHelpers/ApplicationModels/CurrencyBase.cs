using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyExchangeHelpers.ApplicationModels
{
    public class CurrencyBase
    {
        public string BaseCurrency { get; set; }
        public double BaseAmount { get; set; }
        public string ConvertCurrency { get; set; }
        public double ConvertedAmount { get; set; }

        public DateTime ExchangeRateDate { get; set; }
    }
}
