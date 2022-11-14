using System.ComponentModel.DataAnnotations;

namespace DailySpendBudgetWebApp.Models
{
    public class UserHomePageModel
    {
        [Required]
        public string Email { get; set; }
        public int DefaultBudget { get; set; }
        public string? NickName { get; set; }
    }
}
