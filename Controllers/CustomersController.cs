using CSW305Proj.Data;
using CSW305Proj.DTOs;
using CSW305Proj.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSW305Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CSW306DBContext _context;
        public CustomersController(CSW306DBContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<List<CustomerDto>> GetCustomers()
        {
            return await _context.Customers
                .Include(c => c.User)
                .Select(c => new CustomerDto
                {
                    CustomerId = c.CustomerId,
                    FullName = c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    CreatedDate = c.CreatedDate,
                    IdentityCard = c.IdentityCard,
                    Address = c.Address,
                    User = c.User == null ? null : new UserDto
                    {
                        UserId = c.User.UserId,
                        CustomerId = c.User.CustomerId,
                        UserName = c.User.UserName,
                        IsActive = c.User.IsActive,
                        IsBlocked = c.User.IsBlocked,
                        CreatedDate = c.User.CreatedDate
                    }
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null)
                return NotFound();

            var dto = new CustomerDto
            {
                CustomerId = customer.CustomerId,
                FullName = customer.FullName,
                PhoneNumber = customer.PhoneNumber,
                CreatedDate = customer.CreatedDate,
                IdentityCard = customer.IdentityCard,
                Address = customer.Address,
                User = customer.User == null ? null : new UserDto
                {
                    UserId = customer.User.UserId,
                    CustomerId = customer.User.CustomerId,
                    UserName = customer.User.UserName,
                    IsActive = customer.User.IsActive,
                    IsBlocked = customer.User.IsBlocked,
                    CreatedDate = customer.User.CreatedDate
                }
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto dto)
        {
            var customer = new Customers
            {   
                CustomerId = dto.CustomerId,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                CreatedDate = DateTime.Now,
                IdentityCard = dto.IdentityCard,
                Address = dto.Address


            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, UpdateCustomerDto dto)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            customer.FullName = dto.FullName;
            customer.PhoneNumber = dto.PhoneNumber;
            customer.IdentityCard = dto.IdentityCard;
            customer.Address = dto.Address;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
