using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class lut_DateFormat
    {

        [Key]
        public int id { get; set; }
        public int DateSeperatorID { get; set; }
        public int ShortDatePatternID { get; set; }
        [MaxLength(25)]
        public string DateFormat { get; set; } = "";

    }
}
