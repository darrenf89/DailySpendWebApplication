using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class lut_CurrencyDecimalSeparator
    {

        [Key]
        public int id { get; set; }
        [MaxLength(25)]
        public string CurrencyDecimalSeparator { get; set; } = "";

    }
}
