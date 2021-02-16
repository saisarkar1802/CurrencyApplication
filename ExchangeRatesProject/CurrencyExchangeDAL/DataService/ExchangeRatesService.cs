using CurrencyExchangeDAL.Context;
using CurrencyExchangeDAL.DatabaseModels;
using CurrencyExchangeDAL.DataInterface;
using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchangeDAL.DataService
{
    public class ExchangeRatesService: IExchangeRatesService
    {
        private readonly CurrencyExchangeDBContext currencyExchangeDBContext;

        public ExchangeRatesService(CurrencyExchangeDBContext currencyExchangeDBContext)
        {
            this.currencyExchangeDBContext = currencyExchangeDBContext;
        }

        /// <summary>
        /// Method to Insert new CurrencyMasterData
        /// </summary>
        /// <param name="currencyMaster"></param>
        /// <returns></returns>
        public async Task<long> AddCurrencyMasterAsync(CurrencyMaster currencyMaster)
        {
            await currencyExchangeDBContext.CurrencyMasters.AddAsync(currencyMaster);
            if (await currencyExchangeDBContext.SaveChangesAsync() == 1)
            {
                return currencyMaster.Id;
            }
            else
            {
                return 0;
            }                
        }

        /// <summary>
        /// Method to Insert All ExchangeRates for the day
        /// </summary>
        /// <param name="newExchangeRate"></param>
        /// <returns></returns>
        public async Task AddExchangeRatesAsync(List<ExchangeRatesData> newExchangeRate)
        {            
            await currencyExchangeDBContext.BulkInsertAsync(newExchangeRate);
        }

        /// <summary>
        /// Check if data is already available for a given date
        /// </summary>
        /// <param name="checkDate"></param>
        /// <returns></returns>
        public CurrencyMaster CheckIfAvailable(DateTime checkDate)
        {
            return currencyExchangeDBContext.CurrencyMasters.Where(a => a.ApplicableOn == checkDate).FirstOrDefault();
        }

        public Dictionary<string,string> GetExchangeRatesByTimeFrame(DateTime startDate, DateTime endDate, string currency)
        {
            List<CurrencyMaster> listOfdays = currencyExchangeDBContext.CurrencyMasters
                .Where(a => a.ApplicableOn >= startDate && a.ApplicableOn <= endDate).ToList();
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            foreach(CurrencyMaster curMstr in listOfdays)
            {
                ExchangeRatesData exchangeRatesData = currencyExchangeDBContext.exchangeRates
                    .Where(a => a.CurrencyMasterID == curMstr.Id && a.ConvertedCurrency == currency).FirstOrDefault();                
                keyValuePairs.Add(curMstr.ApplicableOn.ToString("yyyy-MM-dd"),Math.Round(exchangeRatesData.ExchangeRate,1).ToString());
            }
            return keyValuePairs;
        }
    }
}
