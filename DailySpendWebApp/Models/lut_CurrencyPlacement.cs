

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class lut_CurrencyPlacement
    {

        [Key]
        public int id { get; set; }
        [MaxLength(25)]
        public string CurrencyPlacement { get; set; } = "";
        public int CurrencyPositivePatternRef { get; set; } = 0;

    }
}
