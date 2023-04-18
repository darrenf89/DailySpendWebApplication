using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class Transactions
    {
        [Key]
        public int TransactionID { get; set; }
        public string? TransactionType { get; set; }
        public bool isSpendFromSavings { get; set; } = false;
        [ForeignKey("Savings")]
        public int? SavingID { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now.Date;
        public bool isIncome { get; set; } = false;
        public decimal TransactionAmount { get; set; }
        public string? Category { get; set; }
        public string? Payee { get; set; }
        public string? Notes { get; set; }
    }
}
