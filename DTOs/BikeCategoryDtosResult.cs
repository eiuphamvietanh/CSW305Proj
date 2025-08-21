namespace CSW305Proj.DTOs
{
    public class BikeCategoryDtosResult
    {
        public int BikeCategoryId { get; set; }
        public string BikeCategoryName { get; set; }
        public List <BikeDtos> Bikes { get; set; }
    }
}
