using CurrencyExchangeHelpers.ApplicationModels;
using CurrencyExchangeHelpers.HelperInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyExchangeHelpers.HelperService
{
    public class CalculationHelperService: ICalculationHelperService
    {
        /// <summary>
        /// This method checks if a string is double or not
        /// </summary>
        /// <param name="amountToCheck"></param>
        /// <returns>converted double amount</returns>
        public double CheckIfDouble(string amountToCheck)
        {
           double returnAmount = (Double.TryParse(amountToCheck, out double convertedAmount) && convertedAmount>=0) ? convertedAmount :  double.NaN;
            return returnAmount;
        }

        /// <summary>
        /// This method is to Get the amount in the converted currency based on fetched rates
        /// </summary>
        /// <param name="keyValueCurrencyData"></param>
        /// <param name="currencyBase"></param>
        /// <returns></returns>
        public double GetConvertedAmount(Dictionary<string,string> keyValueCurrencyData,CurrencyBase currencyBase)
        {
            if(keyValueCurrencyData.TryGetValue(currencyBase.ConvertCurrency,out string exchangeRate))
            {
                double checkedDouble = CheckIfDouble(exchangeRate);
                if(checkedDouble!=double.NaN)
                {
                    return CheckIfDouble(exchangeRate) * currencyBase.BaseAmount;
                }                
            }
            return double.NaN;
        }

        /// <summary>
        /// This method is to check if the input string is a Datetime or not.
        /// </summary>
        /// <param name="dateToCheck"></param>
        /// <returns></returns>
        public DateTime? CheckIfDatetime(string dateToCheck)
        {
            if (DateTime.TryParseExact(dateToCheck, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime resultDateTime))
            {
                return resultDateTime;
            }
            else
                return null;
        }
    }
}
