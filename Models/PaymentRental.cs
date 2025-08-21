using System.ComponentModel.DataAnnotations;

namespace CSW305Proj.Models
{
    public class PaymentRental
    {
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }    

        public int RentalId { get; set; }
        public Rental Rental { get; set; }
    }
}
