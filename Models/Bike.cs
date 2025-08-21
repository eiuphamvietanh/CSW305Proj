using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSW305Proj.Models
{
    [Table("Bikes")]
    public class Bike
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BikeId { get; set; }
        [Required]
        [StringLength(100), MinLength(2)]
        public string BikeCode { get; set; }    
        [Required]
        [StringLength(100), MinLength(2)] 
        public string BikeName { get; set; }
        [Required]
        [StringLength(100), MinLength(2)]
        public string Model { get; set; }
        [Required]
        [StringLength(100), MinLength(2)]
        public string Status { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int BikeStationId { get; set; }
        [Required]
        public int BikeCategoryId { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime UpdatedDate { get; set; }

        [ForeignKey("BikeStationId")]
        public virtual BikeStation BikeStation { get; set; }

        [ForeignKey("BikeCategoryId")]
        public virtual BikeCategory BikeCategory { get; set; }



    }
}
