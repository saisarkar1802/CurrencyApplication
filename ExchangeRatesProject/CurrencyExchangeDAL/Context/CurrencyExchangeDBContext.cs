using CurrencyExchangeDAL.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyExchangeDAL.Context
{
    public class CurrencyExchangeDBContext:DbContext
    {
        public CurrencyExchangeDBContext(DbContextOptions<CurrencyExchangeDBContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }
        public DbSet<ExchangeRatesData> exchangeRates { get; set; }
        public DbSet<CurrencyMaster> CurrencyMasters { get; set; }
    }
}
