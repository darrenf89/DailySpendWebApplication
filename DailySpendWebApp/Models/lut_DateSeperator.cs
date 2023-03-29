using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class lut_DateSeperator
    {

        [Key]
        public int id { get; set; }
        public string DateSeperator { get; set; } = "";

    }
}
