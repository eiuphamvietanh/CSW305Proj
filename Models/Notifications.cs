using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSW305Proj.Models
{
    [Table("Notifications")]
    public class Notifications
    {


        [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     public int NotificationId { get; set; }
     public int UserId { get; set; }
     public required string Message { get; set; }
     public DateTime CreatedDate { get; set; }

     public required User User { get; set; }
    }
}
