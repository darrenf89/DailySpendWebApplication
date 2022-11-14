using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace DailySpendBudgetWebApp.Models
{
    public class Categories
    {
        [Key]
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public decimal? TotalCategorySpend { get; set; }
        public int? LastTransactionID { get; set; }
        public int? NumberOfTransactions { get; set; }
        public List<Transactions>? Transactions { get; set; }
    }
}
