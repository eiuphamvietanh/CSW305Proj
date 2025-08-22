using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSW305Proj.DTOs
{
    public class DiscountDto
    {
        
        public string DiscountCode { get; set; }

        
        public string Description { get; set; }

     
        public DateTime StartDate { get; set; }

      
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

       
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Value { get; set; }

        

        public int DiscountType { get; set; }
    }
}
