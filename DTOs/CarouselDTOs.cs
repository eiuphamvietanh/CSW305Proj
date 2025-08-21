using CSW305Proj.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSW305Proj.DTOs
{
    public class CarouselDTOs
    {
        [Required]
        [StringLength(255)]
        public string ImageUrl { get; set; }

        [StringLength(50)]

        public int? BikeId { get; set; } // Nullable for role-based images

        public int? BikeCategoryId { get; set; } // Nullable for role-based images

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        [ForeignKey("BikeId")]
        public virtual required Bike Bike { get; set; }

        [ForeignKey("BikeCategoryId")]
        public virtual required BikeCategory BikeCategory { get; set; }
    }
}
