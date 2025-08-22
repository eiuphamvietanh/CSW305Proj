namespace CSW305Proj.DTOs
{
    public class CreatePaymentDtos
    {
       public int UserId { get; set; }
        public int DiscountId { get; set; }
        public string Method { get; set; }
        public string Note { get; set; }

    }
}
