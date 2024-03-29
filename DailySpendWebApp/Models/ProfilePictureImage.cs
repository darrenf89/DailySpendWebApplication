using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailySpendBudgetWebApp.Models
{
    public class ProfilePictureImage
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [ForeignKey("UserAccount")]
        public int UserID { get; set; }
        [Required]
        [MaxLength(200)]
        public string FileLocation { get; set; }
        [Required]
        public DateTime WhenAdded { get; set; }
        [NotMapped]
        public Stream File { get; set; }
        [NotMapped]
        public string FileName { get; set; }
        public UserAccounts? UserAccount { get; set; }
    }   
}
