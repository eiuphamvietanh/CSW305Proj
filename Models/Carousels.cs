using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSW305Proj.Models
{
    public class Carousels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CarouselId { get; set; }

        [Required]
        [StringLength(200), MinLength(2)]
        public required string Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public required string ImageUrl { get; set; }

        // For Bike-based carousel
        public int? BikeId { get; set; }
        public int? BikeCategoryId { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public int SortOrder { get; set; } = 0;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("BikeId")]
        public virtual Bike? Bike { get; set; }

        [ForeignKey("BikeCategoryId")]
        public virtual BikeCategory? BikeCategory { get; set; }
    }
}
