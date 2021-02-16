using CurrencyExchangeAPI.Models;
using CurrencyExchangeDAL.DataInterface;
using CurrencyExchangeHelpers.ApplicationModels;
using CurrencyExchangeHelpers.HelperInterface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI
{
    public class APIHelperService
    {
        private IConfiguration _configuration;
        ICalculationHelperService _calculationHelper;
        IAPIInteractionService _aPIInteraction;
        IExchangeRatesService _exchangeRatesService;
        public APIHelperService(IConfiguration configuration, IExchangeRatesService exchangeRatesService,
        ICalculationHelperService calculationHelper, IAPIInteractionService aPIInteraction)
        {
            _configuration = configuration;
            _calculationHelper = calculationHelper;
            _aPIInteraction = aPIInteraction;
            _exchangeRatesService = exchangeRatesService;
        }

        /// <summary>
        /// This method is to fetch latest rates
        /// </summary>
        /// <param name="baseCur"></param>
        /// <param name="toCur"></param>
        /// <param name="baseAmount"></param>
        /// <returns></returns>
        public async Task<ExchangeAPIResponse> GetLatestConversion(string baseCur,string toCur,string baseAmount)
        {
            ExchangeAPIResponse exchangeResponse = new ExchangeAPIResponse();
            CurrencyBase currencyBase = new CurrencyBase();
            currencyBase.BaseAmount = _calculationHelper.CheckIfDouble(baseAmount);
            if (!double.IsNaN(currencyBase.BaseAmount))
            {
                currencyBase.BaseCurrency = baseCur;
                currencyBase.ConvertCurrency = toCur;
                APIResponse aPIResponse =
                await _aPIInteraction.GetCurrencyConversionData(currencyBase, _configuration.GetValue<string>("URL"),
                _configuration.GetValue<string>("AccessKey"));
                return SendLatestRatesResponse(aPIResponse, currencyBase); 
            }
            else
            {
                exchangeResponse.IsSuccess = false;
                exchangeResponse.Description = "Base Amount is not a valid number";
                exchangeResponse.StatusCode = HttpStatusCode.BadRequest;
                return exchangeResponse;
            }
        }

        /// <summary>
        /// This method sends the response with the new calculation
        /// </summary>
        /// <param name="aPIResponse"></param>
        /// <param name="currencyBase"></param>
        /// <returns></returns>
        public ExchangeAPIResponse SendLatestRatesResponse(APIResponse aPIResponse, CurrencyBase currencyBase)
        {
            ExchangeAPIResponse exchangeAPIResponse = new ExchangeAPIResponse();
            if (aPIResponse.success)
            {
                exchangeAPIResponse.IsSuccess = true;
                exchangeAPIResponse.StatusCode = HttpStatusCode.OK;
                exchangeAPIResponse.ConvertedAmount = _calculationHelper.GetConvertedAmount(aPIResponse.rates, currencyBase);
                exchangeAPIResponse.Date = (currencyBase.ExchangeRateDate == DateTime.MinValue) ? String.Empty
                    : currencyBase.ExchangeRateDate.ToString("yyyy-MM-dd");
                exchangeAPIResponse.Description = "Successfully found rates";
            }
            else 
            {
                exchangeAPIResponse.IsSuccess = false;
                exchangeAPIResponse.Description = (aPIResponse.error!=null)? aPIResponse.error.info: "There was an error processing the data";
                exchangeAPIResponse.StatusCode = HttpStatusCode.BadRequest;
            }
            return exchangeAPIResponse;
        }

        /// <summary>
        /// Fetches the rates based on historical date
        /// </summary>
        /// <param name="baseCur"></param>
        /// <param name="toCur"></param>
        /// <param name="baseAmount"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<ExchangeAPIResponse> GetHistoricalRates(string baseCur, string toCur, string baseAmount,string date)
        {
            ExchangeAPIResponse exchangeResponse = new ExchangeAPIResponse();
            CurrencyBase currencyBase = new CurrencyBase();
            currencyBase.BaseAmount = _calculationHelper.CheckIfDouble(baseAmount);
            DateTime? checkDate = _calculationHelper.CheckIfDatetime(date);
            if(checkDate!=null && !double.IsNaN(currencyBase.BaseAmount))
            {
                currencyBase.BaseCurrency = baseCur;
                currencyBase.ConvertCurrency = toCur;
                currencyBase.ExchangeRateDate = Convert.ToDateTime(checkDate);
                APIResponse aPIResponse =
                await _aPIInteraction.GetCurrencyConversionDataByDate(currencyBase, _configuration.GetValue<string>("URL"),
                _configuration.GetValue<string>("AccessKey"));
                return SendLatestRatesResponse(aPIResponse, currencyBase);
            }
            else
            {
                exchangeResponse.IsSuccess = false;
                exchangeResponse.Description = "Parameters passed are not correct";
                exchangeResponse.StatusCode = HttpStatusCode.BadRequest;
                return exchangeResponse;
            }
        }

        public async Task<ExchangeAPIResponse> GetHistoricalDataByTimeFrame(string toCur, string startDate, string endDate)
        {
            ExchangeAPIResponse exchangeAPIResponse = new ExchangeAPIResponse();
            DateTime? checkStartDate = _calculationHelper.CheckIfDatetime(startDate);
            DateTime? checkEndDate = _calculationHelper.CheckIfDatetime(endDate);
            if(checkStartDate!=null && checkEndDate!=null)
            {
                exchangeAPIResponse.IsSuccess = true;
                exchangeAPIResponse.StatusCode = HttpStatusCode.OK;
                exchangeAPIResponse.ExchangeRatesByDate = 
                    _exchangeRatesService.GetExchangeRatesByTimeFrame((DateTime)checkStartDate, (DateTime)checkEndDate, toCur);
            }
            return exchangeAPIResponse;
        }
    }
}
