using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSW305Proj.Models
{
    public class Discount
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DiscountId { get; set; }

        [Required]
        [MaxLength(100)]
        public string DiscountCode { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Value { get; set; }

        [Required]
        
        public int DiscountType { get; set; }
    }
}
