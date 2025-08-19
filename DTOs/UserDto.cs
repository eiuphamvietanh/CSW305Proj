namespace CSW305Proj.DTOs
{
  
    

        public class UserDto
        {
            public int UserId { get; set; }   
            public int CustomerId { get; set; }
            public string UserName { get; set; }
            public bool IsActive { get; set; }
            public bool IsBlocked { get; set; }
            public DateTime CreatedDate { get; set; }
        }

        public class CreateUserDto
        {
            public int CustomerId { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
        }   

        public class UpdateUserDto
        {
            public string UserName { get; set; }
            public bool IsActive { get; set; }
            public bool IsBlocked { get; set; }
        }
    
}
