using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class lut_CurrencySymbol
    {

        [Key]
        public int id { get; set; }
        public string CurrencySymbol { get; set; } = "";

    }
}
