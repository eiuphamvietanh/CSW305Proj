using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSW305Proj.Models
{
    [Table("BikeCategory")]

    public class BikeCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  BikeCategoryId { get; set; }
        [Required]
        public string BikeCategoryName { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public DateTime   CreatedDate { get; set; } = DateTime.Now;
        [Required]
        public DateTime UpdatedDate { get; set; }
        [Required]
        public bool IsActived { get; set; }

        public List<Bike> Bikes { get; set; } = new();

    }
}
