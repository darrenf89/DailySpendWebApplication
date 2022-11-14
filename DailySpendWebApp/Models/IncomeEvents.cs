using System.ComponentModel.DataAnnotations;

namespace DailySpendBudgetWebApp.Models
{
    public class IncomeEvents
    {
        [Key]
        public int IncomeEventID { get; set; }
        public decimal IncomeAmount { get; set; }
        public DateTime DateOfIncomeEvent { get; set; } = DateTime.Now;
        public string? IncomeType { get; set; }
        public int IncomeEventDuration { get; set; }

    }
}
