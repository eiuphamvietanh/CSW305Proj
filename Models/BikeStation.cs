using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSW305Proj.Models
{
    [Table("BikeStations")]
    public class BikeStation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StationId { get; set; }
       
        [StringLength(100, ErrorMessage = "The maximum length is 100")]
        [Required]
        public string StationName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The maximum length is 100")]
        public string Location { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Capacity must be greater than 0")]
        public int Capacity  { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }   
        [Required]
        public DateTime UpdatedDate { get; set; }

        [Required]
        public bool IsActived { get; set; }
        public List<Bike> Bikes { get; set; }



    }
}
