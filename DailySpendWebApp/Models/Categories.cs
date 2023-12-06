using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace DailySpendBudgetWebApp.Models
{
    public class Categories
    {
        [Key]
        public int CategoryID { get; set; }
        [MaxLength(25)]
        public string? CategoryName { get; set; }
        public bool isSubCategory { get; set; } = true;
        public int? CategoryGroupID { get; set; }

    }
}
