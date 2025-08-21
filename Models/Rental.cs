using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSW305Proj.Models
{
    [Table("Rentals")]
    public class Rental
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RentalId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int BikeId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime DuedTime { get; set; }

        public DateTime? ReturnedTimme { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }
        public Bike Bike { get; set; }
        public User User { get; set; }
        public List<PaymentRental> PaymentRentals { get; set; }
    }
}
