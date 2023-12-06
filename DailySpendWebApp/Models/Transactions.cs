using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class Transactions
    {
        [Key]
        public int TransactionID { get; set; }
        public bool isSpendFromSavings { get; set; } = false;
        [ForeignKey("Savings")]
        public int? SavingID { get; set; }
        [MaxLength(25)]
        public string? SavingName { get; set; } = "";
        public DateTime? TransactionDate { get; set; }
        public DateTime? WhenAdded { get; set; } = DateTime.UtcNow;
        public bool isIncome { get; set; } = false;
        public decimal? TransactionAmount { get; set; }
        [MaxLength(25)]
        public string? Category { get; set; } = "";
        public string? Payee { get; set; } = "";
        [MaxLength(250)]
        public string? Notes { get; set; }
        [ForeignKey("Categories")]
        public int? CategoryID { get; set; }
        public bool isTransacted { get; set; } = false;
        [MaxLength(50)]
        public string? SavingsSpendType { get; set; }
        [NotMapped]
        public List<string> PayeeList { get; set; } = new List<string>();
        [MaxLength(25)]
        public string stage { get; set; } = "Create";
        [MaxLength(25)]
        public string EventType { get; set; }
    }
}
