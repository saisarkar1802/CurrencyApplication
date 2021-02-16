using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CurrencyExchangeDAL.DatabaseModels
{
	[Table("ExchangeRatesData")]
	public class ExchangeRatesData
	{
		[Key]
		public long Id { get; set; }
		public long CurrencyMasterID { get; set; }
		public string BaseCurrency { get; set; }
		public string ConvertedCurrency { get; set; }
		public double ExchangeRate { get; set; }
		public DateTime UpdatedDate { get; set; }
	}
}
