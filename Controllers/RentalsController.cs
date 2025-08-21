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
    public class RentalsController : ControllerBase
    {
        private readonly CSW306DBContext _context;
        public RentalsController(CSW306DBContext context) { 
           _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CreateRental([FromBody] CreateRentalRequest request)
        {
            var rental = new Rental
            {
                UserId = request.UserId,
                BikeId = request.BikeId,
                StartTime = DateTime.Now,
                DuedTime = request.DuedTime,
                Status = "Ongoing"
            };

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return Ok(rental);
        }

        // GET: api/rental
        [HttpGet]
        public async Task<IActionResult> GetAllRentals()
        {
            var rentals = await _context.Rentals.ToListAsync();
            return Ok(rentals);
        }

        // GET: api/rental/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRentalById(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);

            if (rental == null)
                return NotFound();

            return Ok(rental);
        }

        // GET: api/rental/user/id
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRentalsByUser(int userId)
        {
            var rentals = await _context.Rentals
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return Ok(rentals);
        }

        // PUT: api/rental/return/id
        [HttpPut("return/{id}")]
        public async Task<IActionResult> ReturnRental(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
                return NotFound();

            rental.ReturnedTimme = DateTime.Now;
            rental.Status = "Returned";

            await _context.SaveChangesAsync();

            return Ok(rental);
        }
    }
}
