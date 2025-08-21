using System.ComponentModel.DataAnnotations;

namespace CSW305Proj.DTOs
{
    public class BikeStationDtos
    {
        [Required]
        [StringLength(100 , ErrorMessage = "The maximum length is 100")]
        public string StationName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The maximum length is 100")]
        public string Location { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage =  "Capacity must be greater than 0")]
        public int Capacity { get; set; }
        [Required]
        public bool IsActived { get; set; }

    }
}
