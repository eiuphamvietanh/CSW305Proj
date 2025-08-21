using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSW305Proj.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public int UserId { get; set; }
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }
        [Required]
        [StringLength(255)]
        public string Password { get; set; }
        [Required]
        public bool IsActived { get; set; }
        [Required]
        public bool IsBlocked { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        [Required]
        [StringLength(100)]
        public string Phone { get; set; }
        [Required]
        [StringLength(100)]
        public string Address { get; set; }
        [Required]
        public int IdentityCard { get; set; }

        public List<Notifications> Notifications { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public List<Rental> Rentals { get; set; }
    }
}
