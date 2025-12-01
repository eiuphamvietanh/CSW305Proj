using System.ComponentModel.DataAnnotations;

namespace CSW305Proj.DTOs
{
    public class BikeDtoResult
    {
        public int BikeId { get; set; }
        public string BikeCode { get; set; }
        public string BikeName { get; set; }
        public string Model { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public BikeCategoryDtos BikeCategory { get; set; }
        public BikeStationDtos BikeStation { get; set; }    
    }
}
