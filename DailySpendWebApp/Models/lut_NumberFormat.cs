using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class lut_NumberFormat
    {

        [Key]
        public int id { get; set; }
        public int CurrencyDecimalDigitsID { get; set; }
        public int CurrencyDecimalSeparatorID { get; set; } 
        public int CurrencyGroupSeparatorID { get; set; }
        [MaxLength(25)]
        public string NumberFormat { get; set; } = "";

    }
}
