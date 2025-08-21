using System.ComponentModel.DataAnnotations;

namespace CSW305Proj.DTOs
{
    public class BikeDtos
    {
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
       
    }
}
