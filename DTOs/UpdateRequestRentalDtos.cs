namespace CSW305Proj.DTOs
{
    public class UpdateRequestRentalDtos
    {
        public int UserId { get; set; }
        public int BikeId { get; set; }
        public string RentalStatus { get; set; }  
        public string BikeStatus { get; set; }
    }
}
