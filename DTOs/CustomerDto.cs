using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSW305Proj.DTOs
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public int IdentityCard { get; set; }
        public string Address { get; set; }
        public UserDto? User { get; set; }
    }

    public class CreateCustomerDto
    {
       
        public int CustomerId { get; set; }
   
        public string FullName { get; set; }
       
        public string PhoneNumber { get; set; }
      
        public DateTime CreatedDate { get; set; }
      
        public int IdentityCard { get; set; }

        public string Address { get; set; }
    }

    public class UpdateCustomerDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public int IdentityCard { get; set; }
        public string Address { get; set; }
    }

   
}
