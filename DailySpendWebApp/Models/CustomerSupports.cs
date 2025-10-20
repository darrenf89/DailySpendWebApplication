using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DailySpendBudgetWebApp.Models
{
    public class CustomerSupport
    {
        [Key]
        public int SupportID { get; set; }
        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000)]
        public string Details { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string? PhoneNumber { get; set; }        
        [MaxLength(200)]
        public string? FileName { get; set; }        
        [MaxLength(100)]
        public string? FileLocation { get; set; }
        [Required]
        public DateTime Whenadded { get; set; } = DateTime.UtcNow;
        [Required]
        public bool IsClosed { get; set; } = false;
        public List<CustomerSupportMessage> Replys { get; set; } = new List<CustomerSupportMessage>();

    }

    public class CustomerSupportMessage
    {
        [Key]
        public int MessageID { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Message { get; set; } = string.Empty;
        [Required]
        public DateTime Whenadded { get; set; } = DateTime.UtcNow;
        [Required]
        public bool IsRead { get; set; } = false;
        [Required]
        public bool IsCustomerReply { get; set; }=false;
    }
}
