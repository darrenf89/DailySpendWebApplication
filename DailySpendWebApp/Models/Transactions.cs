using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class Transactions
    {
        [Key]
        public int TransactionID { get; set; }
        public bool isSpendFromSavings { get; set; } = false;
        public DateTime TransactionDate { get; set; }=DateTime.Now;
        public decimal TransactionAmount { get; set; }
        public string? Notes { get; set; }
        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public Categories? Category { get; set; }

    }
}
