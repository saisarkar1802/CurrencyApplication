using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CurrencyExchangeDAL.DatabaseModels
{
    [Table("CurrencyMaster")]
    public class CurrencyMaster
    {        
        public long Id { get; set; }
        public DateTime ApplicableOn { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
