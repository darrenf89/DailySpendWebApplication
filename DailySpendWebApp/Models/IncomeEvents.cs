using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class IncomeEvents
    {
        [Key]
        public int IncomeEventID { get; set; }
        public decimal IncomeAmount { get; set; }
        [MaxLength(50)]
        public string IncomeName { get; set; } = "";
        public DateTime IncomeActiveDate { get; set; } = DateTime.Now;
        public DateTime DateOfIncomeEvent { get; set; } = DateTime.Now;
        public bool isRecurringIncome { get; set; }
        [MaxLength(25)]
        public string? RecurringIncomeType { get; set; } 
        public int? RecurringIncomeValue { get; set; }
        [MaxLength(25)]
        public string? RecurringIncomeDuration { get; set; }
        public bool isClosed { get; set; } = true;
        public bool? isInstantActive { get; set; }
        public bool? isIncomeAddedToBalance { get; set; } = false;
        [ForeignKey("BankAccounts")]
        public int? AccountID { get; set; }
    }
}
