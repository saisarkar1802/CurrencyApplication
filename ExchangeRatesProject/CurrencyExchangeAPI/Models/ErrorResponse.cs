using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI.Models
{
    public class ErrorResponse
    {
        
        public ErrorResponse()
        {
            SystemStatus = "Error";
        }        
        public string SystemStatus;
        public string Description;
    }
}
