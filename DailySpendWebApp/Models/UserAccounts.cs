using System.ComponentModel.DataAnnotations;


namespace DailySpendBudgetWebApp.Models
{
    public class UserAccounts
    {
        [Key]
        public int UserID { get; set; }
        public int UniqueUserID { get; set; }
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
        public bool isDPAPermissions { get; set; } = false;
        public bool isAgreedToTerms { get; set; } = false;
        public bool isEmailVerified { get; set; } = false;
        public int? DefaultBudgetID { get; set; }         
        public int? PreviousDefaultBudgetID { get; set; }        
        [MaxLength(15)]
        public string? SubscriptionType {  get; set; }
        public DateTime SubscriptionExpiry {  get; set; }
        public List<Budgets>? Budgets { get; set; } = new List<Budgets>();
        public List<CustomerSupport> Supports { get; set; } = new List<CustomerSupport>();
        [MaxLength(50)]
        public string ProfilePicture { get; set; }
        public bool HasFamiltyAccounts { get; set; } = false;


    }
}
