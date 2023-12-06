using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class lut_CurrencyGroupSeparator
    {

        [Key]
        public int id { get; set; }
        [MaxLength(25)]
        public string CurrencyGroupSeparator { get; set; } = "";

    }
}
