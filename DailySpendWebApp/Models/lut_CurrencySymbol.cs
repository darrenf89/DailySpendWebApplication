using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class lut_CurrencySymbol
    {

        [Key]
        public int id { get; set; }
        [MaxLength(25)]
        public string CurrencySymbol { get; set; } = "";
        [MaxLength(250)]
        public string Name { get; set; } = "";
        [MaxLength(250)]
        public string Code { get; set; } = "";
    }
}
