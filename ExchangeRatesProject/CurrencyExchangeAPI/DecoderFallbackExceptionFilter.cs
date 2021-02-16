using CurrencyExchangeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI
{
    public class DecoderFallbackExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            ErrorResponse obj = new ErrorResponse();

            if (context.Exception.GetType() == typeof(DecoderFallbackException))
            {
                obj.Description = "Bad Encoding";
            }
            context.Result = new BadRequestObjectResult(obj);
        }
    }
}
