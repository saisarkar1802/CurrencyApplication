using CurrencyExchangeHelpers.ApplicationModels;
using CurrencyExchangeHelpers.HelperInterface;
using CurrencyExchangeHelpers.HelperService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace CurrencyConsoleApplication
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
                //Adding Application classes
                .AddTransient<CurrencyExchangeRate>()
                .AddTransient<ICalculationHelperService,CalculationHelperService>()
                .AddTransient<IAPIInteractionService,APIInteractionService>()
                .BuildServiceProvider();
            serviceProvider.GetService<CurrencyExchangeRate>().InitiateConsoleApp().Wait();
        }

        
    }
}
