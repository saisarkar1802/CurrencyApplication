using CurrencyExchangeDAL;
using CurrencyExchangeDAL.Context;
using CurrencyExchangeDAL.DataInterface;
using CurrencyExchangeDAL.DataService;
using CurrencyExchangeHelpers.HelperInterface;
using CurrencyExchangeHelpers.HelperService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace FetchCurrencyDataJob
{
    class Program
    {
        static void Main(string[] args)
        {
            //Use appsetting for configurable data
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .Build();

            //Adding DI
            var serviceProvider = new ServiceCollection()
                //Adding Logging
                .AddLogging(configure => configure.AddConsole())
                //Adding configuration
                .AddSingleton<IConfiguration>(config)
                //Adding DBContext
                .AddDbContextPool<CurrencyExchangeDBContext>(options =>
                {
                    options.UseSqlServer(config.GetConnectionString("ExchangeRatesDBConnection"));
                })
                .AddScoped<IExchangeRatesService, ExchangeRatesService>()
                //Adding Application classes
                .AddTransient<FetchExchangeData>()
                .AddTransient<ICalculationHelperService, CalculationHelperService>()
                .AddTransient<IAPIInteractionService, APIInteractionService>()
                .BuildServiceProvider();
            serviceProvider.GetService<FetchExchangeData>().FetchLatestCurrencyData().Wait();
        }
    }
}
