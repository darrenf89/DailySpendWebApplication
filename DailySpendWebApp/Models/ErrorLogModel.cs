using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class ErrorLog
    {
        [Key]
        public int ErrorLogID { get; set; }
        public string? ErrorMessage { get; set;}
        [MaxLength(100)]
        public string? ErrorPage { get; set; }
        [MaxLength(100)]
        public string? ErrorMethod { get; set; }
        [MaxLength(100)]
        public string? DeviceName { get; set; }
        [MaxLength(100)]
        public string? DevicePlatform { get; set; }
        [MaxLength(100)]
        public string? DeviceIdiom { get; set; }
        [MaxLength(100)]
        public string? DeviceOSVersion { get; set; }
        [MaxLength(100)]
        public string? DeviceModel { get; set; }
        public int BudgetID { get; set; }
        public DateTime WhenAdded { get; set; }

    }
}