using FetchCurrencyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FetchCurrencyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        protected IConfiguration _configuration = null;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Main Index Action method triggered when the application is launched
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Action method for historical converter
        /// </summary>
        /// <returns></returns>
        public IActionResult HistoricalConverter()
        {
            return View();
        }

        /// <summary>
        /// HTTPPost Action method for historical converter
        /// </summary>
        /// <param name="appModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult HistoricalConverter(AppModel appModel)
        {
            if (ModelState.IsValid)
            {
                string uRl = _configuration.GetValue<string>("URL") + "GetHistoricalData?baseCur=" + appModel.baseCurrency + "&toCur="
                    + appModel.toCurrency + "&baseAmount=" + appModel.baseAmount+ "&date="+appModel.date.ToString("yyyy-MM-dd");
                ExchangeAPIResponse exchangeAPIResponse = GetExchangeAPIResponse(uRl);
                if (exchangeAPIResponse != null && exchangeAPIResponse.IsSuccess)
                {
                    appModel.convertedAmount = exchangeAPIResponse.ConvertedAmount;
                    TempData["AppModel"] = JsonConvert.SerializeObject(appModel);
                    return RedirectToAction("ShowConvertedData");
                }
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Action method for Latest converter
        /// </summary>
        /// <returns></returns>
        public IActionResult LatestConverter()
        {
            return View();
        }

        /// <summary>
        /// HTTPPost Action method for Latest converter
        /// </summary>
        /// <param name="appModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LatestConverter(AppModel appModel)
        {
            if(ModelState.IsValid)
            {
                string uRl = _configuration.GetValue<string>("URL") + "GetLatestData?baseCur=" + appModel.baseCurrency + "&toCur="
                    + appModel.toCurrency + "&baseAmount=" + appModel.baseAmount;
                ExchangeAPIResponse exchangeAPIResponse = GetExchangeAPIResponse(uRl);
                if(exchangeAPIResponse!= null && exchangeAPIResponse.IsSuccess)
                {
                    appModel.convertedAmount = exchangeAPIResponse.ConvertedAmount;
                    TempData["AppModel"] = JsonConvert.SerializeObject(appModel);
                    return RedirectToAction("ShowConvertedData");
                }
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Action method to show the converted amount with latest and historical rates
        /// </summary>
        /// <returns></returns>
        public IActionResult ShowConvertedData()
        {
            if (TempData["AppModel"] != null)
            {
                AppModel appModel = JsonConvert.DeserializeObject<AppModel>((string)TempData["AppModel"]);
                return View(appModel);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Action method for HistoricalAnalysis of data
        /// </summary>
        /// <returns></returns>
        public IActionResult HistoricalDataByTimeFrame()
        {            
            return View();
        }
        
        /// <summary>
        /// Method to retrieve data from the WebAPI for Historical Converter and show in graph
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult GetData(HistoricalTimeFrameModel model)
        {
            string uRl = _configuration.GetValue<string>("URL") + "GetHistoricalDataByTimeFrame?toCur=" + model.toCurrency + "&startDate="
                    + model.startDate.ToString("yyyy-MM-dd") + "&endDate=" + model.endDate.ToString("yyyy-MM-dd");
            ExchangeAPIResponse exchangeAPIResponse = GetExchangeAPIResponse(uRl);
            if (exchangeAPIResponse != null && exchangeAPIResponse.IsSuccess)
            {
                var queryGraph = exchangeAPIResponse.ExchangeRatesByDate.Select(a => new { date = a.Key, rate = Convert.ToDouble(a.Value) }).ToList();
                return Json(queryGraph);
            }
            return RedirectToAction("Index");
        }

        

        /// <summary>
        /// Method to Fetch response from the Web API
        /// </summary>
        /// <param name="uRl"></param>
        /// <returns></returns>
        public ExchangeAPIResponse GetExchangeAPIResponse(string uRl)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    response = client.GetAsync(new Uri(uRl)).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<ExchangeAPIResponse>(response.Content.ReadAsStringAsync().Result);
                    }                    
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error getting GetExchangeAPI"+ex.Message);
                }
                return null;
            }
        }

        /// <summary>
        /// Default Error Action Method
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
