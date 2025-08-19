using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSW305Proj.Models
{
    public class Customers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public int IdentityCard {  get; set; }
        [Required]
        public string Address { get; set; }

        public Users User   { get; set; }
    }
}
