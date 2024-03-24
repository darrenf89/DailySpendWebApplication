
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class lut_BudgetTimeZone
    {

        [Key]
        public int TimeZoneID { get; set; }
        [MaxLength(50)]
        public string TimeZoneName { get; set; }
        [MaxLength(100)]
        public string TimeZoneDisplayName { get; set; }
        public int TimeZoneUTCOffset { get; set; }

    }
}
