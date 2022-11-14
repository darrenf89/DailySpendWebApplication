using DailySpendBudgetWebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Text;
using System.Security.Cryptography;

namespace DailySpendBudgetWebApp.Models
{
    public class UserAccounts
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        [MaxLength(40)]
        public string Email { get; set; } = string.Empty;
        [MaxLength(10)]
        public string? NickName { get; set; }
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Salt { get; set; } = string.Empty;
        [Required]
        public DateTime AccountCreated { get; set; } = DateTime.Now;
        [Required]
        public DateTime? LastLoggedOn { get; set; }
        [Required]
        public bool isDPAPermissions { get; set; } = false;
        public bool isAgreedToTerms { get; set; } = false;
        public bool isEmailVerified { get; set; } = false;
        [ForeignKey("Budget")]
        public int? DefaultBudgetID { get; set; }
        public Budgets? Budget { get; set; }

        public List<Budgets> Budgets { get; set; } = new List<Budgets>();


    }
}
