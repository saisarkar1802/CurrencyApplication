using CurrencyExchangeHelpers.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyExchangeHelpers.HelperInterface
{
    public interface ICalculationHelperService
    {
        public double CheckIfDouble(string amountToCheck);
        public DateTime? CheckIfDatetime(string dateToCheck);
        public double GetConvertedAmount(Dictionary<string, string> keyValueCurrencyData, CurrencyBase currencyBase);
    }
}
