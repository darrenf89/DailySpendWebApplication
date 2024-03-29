using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class OTP
    {
        [Key]
        public int OTPID { get; set; }
        [MaxLength(8)]
        public string OTPCode { get; set; }
        public DateTime OTPExpiryTime { get; set; }
        [ForeignKey("UserAccount")]
        public int UserAccountID { get; set; }
        public bool IsValidated { get; set; }
        [MaxLength(25)]
        public string OTPType { get; set; }
        public UserAccounts? UserAccount { get; set; }
    }

}
