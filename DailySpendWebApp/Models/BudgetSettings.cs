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
        public string? CurrencyPattern { get; set; } = "$n";
        [MaxLength(5)]
        public string? CurrencySymbol { get; set; } = "£";
        public int? CurrencyDecimalDigits { get; set; } = 2;
        [MaxLength(5)]
        public string? CurrencyDecimalSeparator { get; set; } = ".";
        [MaxLength(5)]
        public string? CurrencyGroupSeparator { get; set; } = ",";
        [MaxLength(5)]
        public string? DateSeperator { get; set; } = "/";
        [MaxLength(10)]
        public string? ShortDatePattern { get; set; } = "dd/mm/yyyy";

    }
}
