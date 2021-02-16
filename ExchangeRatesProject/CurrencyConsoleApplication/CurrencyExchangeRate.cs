using CurrencyExchangeHelpers.ApplicationModels;
using CurrencyExchangeHelpers.HelperInterface;
using CurrencyExchangeHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConsoleApplication
{
    public class CurrencyExchangeRate
    {
        private readonly ILogger<CurrencyExchangeRate> _logger;
        private IConfiguration _configuration;
        ICalculationHelperService _calculationHelper;
        IAPIInteractionService _aPIInteraction;
        public CurrencyExchangeRate(ILogger<CurrencyExchangeRate> logger, IConfiguration configuration,
            ICalculationHelperService calculationHelper, IAPIInteractionService aPIInteraction)
        {
            _logger = logger;
            _configuration = configuration;
            _calculationHelper = calculationHelper;
            _aPIInteraction = aPIInteraction;
        }

        /// <summary>
        /// This is the method to initiate the Console App task
        /// </summary>
        /// <returns></returns>
        public async Task InitiateConsoleApp()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("-------------------------Welcome to Currency Calculator-----------------");
            Console.ResetColor();
            Console.WriteLine("Chose usage: \n 1. Currency Conversion \n 2. Historical Currency Conversion");
            switch(Console.ReadKey().Key)
            {
                case ConsoleKey.D1: await CurrencyConversion();
                    return;
                case ConsoleKey.D2: await HistoricalCurrencyConversion();
                    return;
                default:            Console.WriteLine("\n Wrong key selection, exiting app. Press Enter..");
                    Console.ReadKey();
                    return;
            }
            //Console.WriteLine(_configuration.GetValue<string>("Test"));
        }

        /// <summary>
        /// This method is to execute current date currency conversion
        /// </summary>
        /// <returns></returns>
        public async Task CurrencyConversion()
        {
            CurrencyBase curBase = new CurrencyBase();
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("-------------------------Fetch latest currency data-----------------");
            Console.ResetColor();
            string inputBaseCurrencyMessage = "Please enter base currency data in 3 character ISO format[Example:'EUR']";
            curBase.BaseCurrency = GetCurrencyCode(inputBaseCurrencyMessage);
            string baseCurrencyMessage = "Please enter amount in {0}:"+ curBase.BaseCurrency;
            curBase.BaseAmount = ReadDouble(baseCurrencyMessage);
            string inputConvertedCurrencyMessage = "Please enter converted currency data in 3 character ISO format[Example:'EUR']";
            curBase.ConvertCurrency = GetCurrencyCode(inputConvertedCurrencyMessage);
            APIResponse aPIResponse =
                await _aPIInteraction.GetCurrencyConversionData(curBase, _configuration.GetValue<string>("URL"),
                _configuration.GetValue<string>("AccessKey"));
            ProcessCurrencyCalculationResponse(aPIResponse, curBase);           
        }

        /// <summary>
        /// This method is to show the final output
        /// </summary>
        /// <param name="aPIResponse">Response from the API</param>
        /// <param name="curBase">Input Output data</param>
        public void ProcessCurrencyCalculationResponse(APIResponse aPIResponse, CurrencyBase curBase)
        {
            if (aPIResponse.success)
            {
                curBase.ConvertedAmount = _calculationHelper.GetConvertedAmount(aPIResponse.rates, curBase);
                Console.WriteLine("The converted amount in {0} is " + curBase.ConvertedAmount, curBase.ConvertCurrency);
            }
            else if (aPIResponse.error != null)
            {
                Console.WriteLine("There was an error as:" + aPIResponse.error.info);
            }
            else
            {
                Console.WriteLine("There was an error processing the data");
            }
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        
        /// <summary>
        /// This method to check and allow user to only three character ISO code for currency
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string GetCurrencyCode(string message)
        {
            while(true)
            {
                Console.WriteLine(message);
                string inputCurrency = Console.ReadLine();
                if(!String.IsNullOrEmpty(inputCurrency) && inputCurrency.Length==3)
                {
                    return inputCurrency;
                }
                Console.WriteLine("Currency format is not correct. Please use correct format [Example:'EUR']");
            }
        }

        /// <summary>
        /// This method is to constantly ask user to enter correct double value
        /// </summary>
        /// <param name="message"></param>
        /// <returns>returns correct amount entered in actual value</returns>
        public double ReadDouble(string message)
        {
            while(true)
            {
                Console.WriteLine(message);
                double checkData = _calculationHelper.CheckIfDouble(Console.ReadLine());
                if(!double.IsNaN(checkData) && checkData>=0)
                {
                    return checkData;
                }
                Console.WriteLine("Amount data error. Please enter correct data");
            }
        }
        
        /// <summary>
        /// This method is to execute historical date currency conversion
        /// </summary>
        /// <returns></returns>
        public async Task HistoricalCurrencyConversion()
        {
            CurrencyBase currencyBase = new CurrencyBase();
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("-------------------------Fetch currency data by date-----------------");
            Console.ResetColor();
            string inputBaseCurrencyMessage = "Please enter base currency data in 3 character ISO format[Example:'EUR']";
            currencyBase.BaseCurrency = GetCurrencyCode(inputBaseCurrencyMessage);
            string baseCurrencyMessage = "Please enter amount in {0}:" + currencyBase.BaseCurrency;
            currencyBase.BaseAmount = ReadDouble(baseCurrencyMessage);
            string inputConvertedCurrencyMessage = "Please enter converted currency data in 3 character ISO format[Example:'EUR']";
            currencyBase.ConvertCurrency = GetCurrencyCode(inputConvertedCurrencyMessage);
            Console.WriteLine("Do you want to enter historical date?(Y/N)");
            if(Console.ReadKey().Key==ConsoleKey.Y)
            {
                string dateMessage = "Please enter date in YYYY-MM-DD format";
                currencyBase.ExchangeRateDate = ReadDatetime(dateMessage);
            }
            else
            {
                currencyBase.ExchangeRateDate = DateTime.Now;
            }
            APIResponse aPIResponse =
                await _aPIInteraction.GetCurrencyConversionDataByDate(currencyBase, _configuration.GetValue<string>("URL"),
                _configuration.GetValue<string>("AccessKey"));
            ProcessCurrencyCalculationResponse( aPIResponse, currencyBase);
        }

        /// <summary>
        /// This method is to constantly ask user to enter correct datetime value
        /// </summary>
        /// <param name="message"></param>
        public DateTime ReadDatetime(string message)
        {
            while (true)
            {
                Console.WriteLine(message);
                DateTime? checkData = _calculationHelper.CheckIfDatetime(Console.ReadLine());
                if (checkData!=null)
                {
                    return Convert.ToDateTime(checkData);
                }
                Console.WriteLine("Date data error. Please enter correct date");
            }
        }
    }
}
