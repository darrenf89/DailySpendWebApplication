using DailySpendBudgetWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace DailySpendWebApp.Models
{
    public class FamilyUserAccount
    {
        [Key]
        public int UserID { get; set; }
        public int UniqueUserID { get; set; }
        public int ParentUserID { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;
        [MaxLength(50)]
        public string? NickName { get; set; }
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Salt { get; set; } = string.Empty;
        [Required]
        public DateTime AccountCreated { get; set; } = DateTime.Now;
        [Required]
        public DateTime? LastLoggedOn { get; set; }
        [Required]
        public bool IsDPAPermissions { get; set; } = false;
        public bool IsAgreedToTerms { get; set; } = false;
        public bool IsEmailVerified { get; set; } = false;
        public bool IsActive { get; set; } = false;
        public bool IsConfirmed { get; set; } = false;
        public bool IsBudgetHidden { get; set; } = false;
        public int? BudgetID { get; set; }
        public int? AssignedBudgetID { get; set; }
        [MaxLength(50)]
        public string ProfilePicture { get; set; }
        public List<Budgets> Budgets { get; set; } = new List<Budgets>();
        public List<CustomerSupport> Supports { get; set; } = new List<CustomerSupport>();
    }
}
