using System.ComponentModel.DataAnnotations;

namespace DailySpendBudgetWebApp.Models
{
    public class OTP
    {
        [Key]
        public int OTPID { get; set; }
        [MaxLength(8)]
        public string OTPCode { get; set; }
        public DateTime OTPExpiryTime { get; set; }
        public int UserAccountID { get; set; }
        public bool IsValidated { get; set; }
        [MaxLength(25)]
        public string OTPType { get; set; }
    }

}
