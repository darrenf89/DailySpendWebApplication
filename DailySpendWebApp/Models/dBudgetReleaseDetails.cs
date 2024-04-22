using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class dBudgetReleaseDetails
    {
        [Key]
        public int ReleaseID { get; set; }
        [MaxLength(10)]
        public string VersionName { get; set; } = string.Empty;
        [MaxLength(10)]
        public string VersionNumber { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public bool IsMajorVersion { get; set; }
    }
}
