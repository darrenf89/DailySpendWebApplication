using DailySpendBudgetWebApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendWebApp.Models
{
    public class UserAddDetails
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int UserID { get; set; }
        public DateTime LastViewed {  get; set; }
        public int NumberOfViews { get; set; }

    }
}
