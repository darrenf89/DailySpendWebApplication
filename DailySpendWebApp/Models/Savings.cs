using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class Savings
    {
        [Key]
        public int SavingID { get; set; }
        public string? SavingsName { get; set; }
        public decimal CurrentBalance { get; set; } = 0;
        public DateTime LastUpdatedDate { get; set; } = DateTime.Now;
        public decimal? LastUpdatedValue { get; set; }
        public bool isSavingsClosed { get; set; } = false;

    }
}
