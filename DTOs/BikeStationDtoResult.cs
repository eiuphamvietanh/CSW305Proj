namespace CSW305Proj.DTOs
{
    public class BikeStationDtoResult
    {
        public int StationId { get; set; }
        public string StationName { get; set; }
        public string Location { get; set; }    
        public int Capacity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActived { get; set; }
        public List<BikeDtoResult> Bikes { get; set; }

    }
}
