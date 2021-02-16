using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FetchCurrencyWeb.Models
{
    public class HistoricalTimeFrameModel
    {
        [Display(Name = "Currency")]
        [Required]
        [RegularExpression("^[a-zA-Z ]*$")]
        [StringLength(3, MinimumLength = 3)]
        public string toCurrency { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime startDate { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime endDate { get; set; }
    }
}
