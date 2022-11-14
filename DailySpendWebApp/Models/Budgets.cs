using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class Budgets
    {
        [Key]
        public int BudgetID { get; set; }
        public string? BudgetName { get; set; }
        public DateTime BudgetCreatedOn { get; set; } = DateTime.Now;
        [DataType(DataType.Currency)]
        public decimal? BankBalance { get; set; }
        [DataType(DataType.Currency)]
        public decimal? MoneyAvailableBalance { get; set; }
        [DataType(DataType.Currency)]
        public decimal? LeftToSPendBalance { get; set; }
        [ForeignKey("IncomeEvent")]
        public int? LastIncomeEventId { get; set; }
        public IncomeEvents? IncomeEvent { get; set; }
        public DateTime? NextIncomeEventDate { get; set; }
        public List<IncomeEvents> IncomeEvents { get; set; } = new List<IncomeEvents>();
        public List<Savings> Savings { get; set; } = new List<Savings>();
        public List<Transactions> Transactions { get; set; } = new List<Transactions>();
        public List<Categories> Categories { get; set; } = new List<Categories>();
        public List<Bills> Bills { get; set; } = new List<Bills>();
        public string? CurrencyType { get; set; }


    }
}
