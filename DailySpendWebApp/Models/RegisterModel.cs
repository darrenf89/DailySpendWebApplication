using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace DailySpendBudgetWebApp.Models
{
    public class RegisterModel
    {
        [Required]
        [MaxLength(40)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$",ErrorMessage = "Password must be at least 8 characters long, have an Uppercase, Lowercase, Number  and Special character!")]
        [MaxLength(25)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [DisplayName("Confirm your Password")]
        [Compare("Password", ErrorMessage = "Whoops passwords don't match, please try again dummy!")]
        [MaxLength(25)]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        public bool isDPAPermissions { get; set; } = false;
        public bool isAgreedToTerms { get; set; } = true;
        public bool isEmailUnique { get; set; }
        public string? Salt { get; set; }
        [MaxLength(10)]
        public string? NickName { get; set; }

    }
}
