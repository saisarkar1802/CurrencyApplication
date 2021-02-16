using CurrencyExchangeHelpers.ApplicationModels;
using CurrencyExchangeHelpers.HelperInterface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchangeHelpers.HelperService
{
    public class APIInteractionService: IAPIInteractionService
    {
        /// <summary>
        /// This method is to interact with the currency api of fixer.io and retrieve latest conversion rates
        /// </summary>
        /// <param name="currencyBase">Currency Base data for base currency and converted data</param>
        /// <param name="uRl">URL to check</param>
        /// <returns></returns>
        public async Task<APIResponse> GetCurrencyConversionData(CurrencyBase currencyBase, string apiURL, string accessKey)
        {
            string uRl = apiURL + "latest?access_key=" + accessKey + "&base=" + currencyBase.BaseCurrency + "&symbols=" + currencyBase.ConvertCurrency;           
            return await GetAPIResponse(uRl);
        }

        /// <summary>
        /// This method is to Fetch currency conversion data for historical dates
        /// </summary>
        /// <param name="currencyBase"></param>
        /// <param name="apiURL"></param>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        public async Task<APIResponse> GetCurrencyConversionDataByDate(CurrencyBase currencyBase, string apiURL, string accessKey)
        {
            string uRl = apiURL +currencyBase.ExchangeRateDate.ToString("yyyy-MM-dd") +"?access_key=" + accessKey + "&base=" 
                + currencyBase.BaseCurrency + "&symbols=" + currencyBase.ConvertCurrency;
            return await GetAPIResponse(uRl);
        }
        /// <summary>
        /// This method is to get latest exchange rates for base and converted currency
        /// </summary>
        /// <param name="uRL">ARL of the currency api with query strings</param>
        /// <returns>APIResponse class with API response data</returns>
        public async Task<APIResponse> GetAPIResponse(string uRL)
        {
            APIResponse aPIResponse = new APIResponse();
            using(var client = new HttpClient())
            {
                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    response = await client.GetAsync(new Uri(uRL)).ConfigureAwait(false);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        aPIResponse = JsonConvert.DeserializeObject<APIResponse>(response.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        Error errorMessage = new Error();
                        errorMessage.info = response.Content.ToString();
                        aPIResponse.error = errorMessage;
                    }
                }
                catch (Exception ex)
                {
                    Error error = new Error();
                    error.info = ex.Message;
                    aPIResponse.error = error;
                }
            }
            return aPIResponse;
        }

        public async Task<APIResponse> GetLatestExchangeRateList(string apiURL, string accessKey)
        {
            string uRl = apiURL + "latest?access_key=" + accessKey;
            return await GetAPIResponse(uRl);
        }
    }
}
