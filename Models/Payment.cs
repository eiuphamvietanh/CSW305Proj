using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSW305Proj.Models
{
    public class Payment
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public decimal BeforeAmount { get; set; }

        [Required]
        public decimal AfterAmount { get; set; }
        [Required]
        [MaxLength(100)]
        public string Method { get; set; }
        [Required]
        [MaxLength(100)]
        public string Note { get; set; }

        public DateTime CreatedDate { get; set; }

        // Navigation
        public List<PaymentRental> PaymentRentals { get; set; }
    }
}
