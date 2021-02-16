using CurrencyExchangeAPI.Models;
using CurrencyExchangeHelpers.HelperInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI.Controllers
{
    [Produces("text/JSON")]
    [ApiController]
    [Route("api/exchange")]
    public class ExchangeAPIController : Controller
    {
        private IConfiguration _configuration;
        ICalculationHelperService _calculationHelper;
        IAPIInteractionService _aPIInteraction;
        APIHelperService _aPIHelperService;
        public ExchangeAPIController(IConfiguration configuration,
            ICalculationHelperService calculationHelper, IAPIInteractionService aPIInteraction,
            APIHelperService aPIHelperService)
        {
            _configuration = configuration;
            _calculationHelper = calculationHelper;
            _aPIInteraction = aPIInteraction;
            _aPIHelperService = aPIHelperService;
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("GetAPIStatus")]
        public async Task<IActionResult> GetAPIStatus()
        {
            return Ok();
        }

        [ProducesResponseType(typeof(ExchangeAPIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("GetLatestData")]
        public async Task<ActionResult> GetLatestData([FromQuery] string baseCur, [FromQuery] string toCur, [FromQuery] string baseAmount)
        {
            ExchangeAPIResponse exchangeAPIResponse = new ExchangeAPIResponse();
            if(!string.IsNullOrEmpty(baseCur)&& !string.IsNullOrEmpty(toCur)&& !string.IsNullOrEmpty(baseAmount))
            {
                 exchangeAPIResponse = await _aPIHelperService.GetLatestConversion(baseCur, toCur, baseAmount);
                if(exchangeAPIResponse.IsSuccess)
                {
                    return Ok(exchangeAPIResponse);
                }
                else
                {                    
                    return BadRequest(exchangeAPIResponse);
                }

            }
            else
            {
                return BadRequest();
            }
        }

        [ProducesResponseType(typeof(ExchangeAPIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("GetHistoricalData")]
        public async Task<ActionResult> GetHistoricalData([FromQuery] string baseCur, [FromQuery] string toCur, 
            [FromQuery] string baseAmount, [FromQuery] string date)
        {
            ExchangeAPIResponse exchangeAPIResponse = new ExchangeAPIResponse();
            if (!string.IsNullOrEmpty(baseCur) && !string.IsNullOrEmpty(toCur) && !string.IsNullOrEmpty(baseAmount) && !string.IsNullOrEmpty(date))
            {
                exchangeAPIResponse = await _aPIHelperService.GetHistoricalRates(baseCur, toCur, baseAmount,date);
                if (exchangeAPIResponse.IsSuccess)
                {
                    return Ok(exchangeAPIResponse);
                }
                else
                {
                    return BadRequest(exchangeAPIResponse);
                }

            }
            else
            {
                return BadRequest();
            }
        }

        [ProducesResponseType(typeof(Dictionary<string,string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("GetHistoricalDataByTimeFrame")]
        public async Task<ActionResult> GetHistoricalDataByTimeFrame([FromQuery] string toCur,[FromQuery] string startDate, [FromQuery] string endDate)
        {
            if (!string.IsNullOrEmpty(toCur) && !string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                ExchangeAPIResponse exchangeAPIResponse = await _aPIHelperService.GetHistoricalDataByTimeFrame(toCur, startDate, endDate);
                if(exchangeAPIResponse.IsSuccess)
                {
                    return Ok(exchangeAPIResponse);
                }
            }
            return BadRequest();
        }
    }
}
