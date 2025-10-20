using System.ComponentModel.DataAnnotations;

namespace DailySpendBudgetWebApp.Models
{
    public class SessionDetails
    {
        [Key]
        public int SessionID { get; set; } = 0;
        [MaxLength(250)]
        public string SessionToken { get; set; } 
        public DateTime SessionExpiry { get; set; }
        public int UserID { get; set; }
        public string SessionUser { get; set; }
        [MaxLength(250)]
        public string RefreshToken { get; set; }
    }
}
