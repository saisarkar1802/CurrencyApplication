using CurrencyExchangeHelpers.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchangeHelpers.HelperInterface
{
    public interface IAPIInteractionService
    {
        public Task<APIResponse> GetCurrencyConversionData(CurrencyBase currencyBase, string apiURL, string accessKey);
        public Task<APIResponse> GetCurrencyConversionDataByDate(CurrencyBase currencyBase, string apiURL, string accessKey);
        public Task<APIResponse> GetLatestExchangeRateList(string apiURL, string accessKey);
    }
}
