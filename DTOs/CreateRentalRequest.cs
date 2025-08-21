namespace CSW305Proj.DTOs
{
    public class CreateRentalRequest
    {
        public int UserId { get; set; }
        public int BikeId { get; set; }
        public DateTime DuedTime { get; set; }
    }
}
