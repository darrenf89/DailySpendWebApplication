

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class lut_CurrencyPlacement
    {

        [Key]
        public int id { get; set; }
        public string CurrencyPlacement { get; set; } = "";

    }
}
