using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class lut_CurrencyDecimalDigits
    {

        [Key]
        public int id { get; set; }
        public string CurrencyDecimalDigits { get; set; } = "";

    }
}
