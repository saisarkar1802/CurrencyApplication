using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FetchCurrencyWeb.Models
{
    public class AppModel
    {
        [Display(Name = "Base Currency")]
        [Required]
        [RegularExpression("^[a-zA-Z ]*$")]
        [StringLength(3,MinimumLength = 3)]
        public string baseCurrency { get; set; }
        [Display(Name = "Base Currency Amount")]
        [Required]
        public double baseAmount { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z ]*$")]
        [StringLength(3, MinimumLength = 3)]
        [Display(Name = "New Currency")]
        public string toCurrency { get; set; }
        
        [Display(Name = "New Currency Amount")]
        public double convertedAmount { get; set; }
        [Display(Name = "Conversion Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime date { get; set; }
    }
}
