using CurrencyExchangeDAL.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchangeDAL.DataInterface
{
    public interface IExchangeRatesService
    {
        public Task<long> AddCurrencyMasterAsync(CurrencyMaster currencyMaster);
        public Task AddExchangeRatesAsync(List<ExchangeRatesData> newExchangeRate);
        public CurrencyMaster CheckIfAvailable(DateTime checkDate);
        public Dictionary<string, string> GetExchangeRatesByTimeFrame(DateTime startDate, DateTime endDate, string currency);
    }
}
