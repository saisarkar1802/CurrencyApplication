using CurrencyExchangeDAL.DatabaseModels;
using CurrencyExchangeDAL.DataInterface;
using CurrencyExchangeHelpers.ApplicationModels;
using CurrencyExchangeHelpers.HelperInterface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FetchCurrencyDataJob
{
    public class FetchExchangeData
    {
        private readonly ILogger<FetchExchangeData> _logger;
        private IConfiguration _configuration;
        IAPIInteractionService _aPIInteraction;
        ICalculationHelperService _calculationHelper;
        IExchangeRatesService _exchangeRatesService;
        public FetchExchangeData(ILogger<FetchExchangeData> logger, IConfiguration configuration, 
            IAPIInteractionService aPIInteraction, ICalculationHelperService calculationHelper, IExchangeRatesService exchangeRatesService)
        {
            _logger = logger;
            _configuration = configuration;
            _aPIInteraction = aPIInteraction;
            _calculationHelper = calculationHelper;
            _exchangeRatesService = exchangeRatesService;
        }

        /// <summary>
        /// Initial Method to Fetch data from API
        /// </summary>
        /// <returns></returns>
        public async Task FetchLatestCurrencyData()
        {
            try
            {
                _logger.LogInformation("Initiating Job");
                if(_exchangeRatesService.CheckIfAvailable(DateTime.Now.Date)==null)
                {
                    APIResponse aPIResponse =
                    await _aPIInteraction.GetLatestExchangeRateList(_configuration.GetValue<string>("URL"),
                    _configuration.GetValue<string>("AccessKey"));
                    long currencyMasterID = await InsertCurrencyMaster(aPIResponse);
                    if (currencyMasterID > 0)
                    {
                        await InsertExchangeRates(currencyMasterID, aPIResponse);
                    }
                }
                else
                {
                    _logger.LogError("Data is available for today in CurrencyMaster");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while fetching data from API as: " + ex.Message);
            }
        }

        /// <summary>
        /// This method is to retrieve CurrencyMasterData from APIResponse and insert to DB
        /// </summary>
        /// <param name="aPIResponse"></param>
        /// <returns></returns>
        public async Task<long> InsertCurrencyMaster(APIResponse aPIResponse)
        {
            try
            {
                CurrencyMaster currencyMaster = new CurrencyMaster();
                DateTime? checkDate = _calculationHelper.CheckIfDatetime(aPIResponse.date);
                currencyMaster.ApplicableOn = (checkDate != null) ? Convert.ToDateTime(checkDate) : DateTime.Now;
                currencyMaster.UpdatedDate = DateTime.Now;
                return await _exchangeRatesService.AddCurrencyMasterAsync(currencyMaster);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while inserting data to database as: " + ex.Message);
                return 0;
            }
        }


        /// <summary>
        /// This method is to retrieve ExchangeRatesData from APIResponse and insert to DB
        /// </summary>
        /// <param name="CurrencyMasterID"></param>
        /// <param name="aPIResponse"></param>
        /// <returns></returns>
        public async Task<long> InsertExchangeRates(long CurrencyMasterID, APIResponse aPIResponse)
        {
            try
            {
                List<ExchangeRatesData> listOfExchanges = new List<ExchangeRatesData>();
                foreach(KeyValuePair<string,string> keyValuePair in aPIResponse.rates)
                {
                    ExchangeRatesData exchangeRates = new ExchangeRatesData();
                    exchangeRates.CurrencyMasterID = CurrencyMasterID;
                    exchangeRates.BaseCurrency = aPIResponse.@base;
                    exchangeRates.ConvertedCurrency = keyValuePair.Key;
                    double exchangeRate = _calculationHelper.CheckIfDouble(keyValuePair.Value);
                    exchangeRates.ExchangeRate = (exchangeRate != double.NaN) ? exchangeRate : 0;
                    exchangeRates.UpdatedDate = DateTime.Now;
                    listOfExchanges.Add(exchangeRates);
                }
                await _exchangeRatesService.AddExchangeRatesAsync(listOfExchanges);
                return 1;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while inserting data in exchangeRates table as: " + ex.Message);
                return 0;
            }
        }
    }
}
