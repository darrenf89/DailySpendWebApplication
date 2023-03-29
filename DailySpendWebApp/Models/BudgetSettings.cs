using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class BudgetSettings
    {

        [Key]
        public int SettingsID { get; set; }
        [ForeignKey("Budget")]
        public int? BudgetID { get; set; }
        public Budgets? Budget { get; set; }
        [MaxLength(5)]
        public int? CurrencyPattern { get; set; } = 1;
        [MaxLength(5)]
        public int? CurrencySymbol { get; set; } = 1;
        public int? CurrencyDecimalDigits { get; set; } = 2;
        [MaxLength(5)]
        public int? CurrencyDecimalSeparator { get; set; } = 1;
        [MaxLength(5)]
        public int? CurrencyGroupSeparator { get; set; } = 1;
        [MaxLength(5)]
        public int? DateSeperator { get; set; } = 1;
        [MaxLength(10)]
        public int? ShortDatePattern { get; set; } = 1;

    }
}
