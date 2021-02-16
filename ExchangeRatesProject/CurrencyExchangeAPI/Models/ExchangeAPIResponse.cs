using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI.Models
{
    public class ExchangeAPIResponse
    {
        
        public bool IsSuccess { get; set; }
        public string Description { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public double ConvertedAmount { get; set; }
        public String Date { get; set; }

        public Dictionary<string,string> ExchangeRatesByDate { get; set; }
    }
}
