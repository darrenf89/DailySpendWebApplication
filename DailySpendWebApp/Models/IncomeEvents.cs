using System.ComponentModel.DataAnnotations;

namespace DailySpendBudgetWebApp.Models
{
    public class IncomeEvents
    {
        [Key]
        public int IncomeEventID { get; set; }
        public decimal IncomeAmount { get; set; }
        public string IncomeName { get; set; } = "";
        public DateTime DateOfIncomeEvent { get; set; } = DateTime.Now;
        public string? IncomeCategory { get; set; }
        public bool isRecurringIncome { get; set; }
        public string RecurringIncomeType { get; set; } = "";
        public int RecurringIncomePeriod { get; set; }  = -1;
        public decimal IncomeDailyValue { get; set; } = -1;
        public bool isActive { get; set; } = true;
        public bool isMainIncome { get; set; } = false;

    }
}
