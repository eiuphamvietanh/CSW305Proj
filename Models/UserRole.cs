using System.ComponentModel.DataAnnotations.Schema;

namespace CSW305Proj.Models
{
    [Table("UserRole")]
    public class UserRole
    {
       public int UserId { get; set; }  
    }
}
