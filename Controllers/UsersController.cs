using CSW305Proj.Data;
using CSW305Proj.DTOs;
using CSW305Proj.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CSW305Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly CSW306DBContext _context;

        public UsersController(CSW306DBContext context)
        {
            _context = context;
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] UserRegisterDtos request)

        //{

        //    if (await _context.Users.AnyAsync(u => u.UserName == request.UserName))
        //        return BadRequest("Username already exists.");

        //    var user = new User
        //    {
        //        UserName = request.UserName,
        //        Password = HashPassword(request.Password),
        //        IsActived = true,
        //        IsBlocked = false,
        //        CreatedDate = DateTime.UtcNow,
        //        FullName = request.FullName,
        //        Phone = request.Phone,
        //        Address = request.Address,
        //        IdentityCard = request.IdentityCard
        //    };

        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return Ok(new { user.UserId, user.UserName });
        //}

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDtos request)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == request.UserName))
                return BadRequest("Username already exists.");

            var user = new User
            {
                UserName = request.UserName,
                Password = HashPassword(request.Password),
                IsActived = true,
                IsBlocked = false,
                CreatedDate = DateTime.UtcNow,
                FullName = request.FullName,
                Phone = request.Phone,
                Address = request.Address,
                IdentityCard = request.IdentityCard,
            };

          
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            
            var customerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Customer");
            if (customerRole == null)
            {
                return BadRequest("Default role 'Customer' does not exist.");
            }

           
            var userRole = new UserRole
            {
                UserId = user.UserId,
                RoleId = customerRole.RoleId
            };

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                user.UserId,
                user.UserName,
                Role = customerRole.RoleName
            });
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users= await _context.Users.ToListAsync();

            return users.Count == 0 ? NotFound("No users found.") : Ok(users.Select(u => new { u.UserId, u.UserName }));  
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.Include(u => u.Rentals).FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null) return NotFound();

            return Ok(new { user.UserId, user.UserName });
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetUser(string name)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == name);

            if (user == null) return NotFound();

            return Ok(new { user.UserId, user.UserName });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserById(int id, UserRegisterDtos request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (request == null)
            {
                return BadRequest("CANNOT BE NULL");
            }
            if (user == null) return NotFound();
            user.UserName = request.UserName;
            user.Password = HashPassword(request.Password);
            user.FullName = request.FullName;
            user.Phone = request.Phone;
            user.Address = request.Address;
            user.IdentityCard = request.IdentityCard;   
            await _context.SaveChangesAsync();

            if (user == null) return NotFound();

            return Ok(new { user.UserId, user.UserName, user.Address, user.FullName, user.Phone, user.IdentityCard});
        }





        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
