using Azure.Core;
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
            var bike = await _context.Bikes.FindAsync(request.BikeId);
            if (bike == null)
                return NotFound("Bike not found.");
            if (bike.Status != "Available") return BadRequest("Bike is not available for rent.");
            var rental = new Rental
            {
                UserId = request.UserId,
                BikeId = request.BikeId,
                StartTime = DateTime.Now,
                DuedTime = request.DuedTime,
                Status = "Ongoing"
            };
            bike.Status = "Rented";     
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();
            var notification = new Notifications
            {
                UserId = request.UserId,
                Message = $"You have successfully rented bike {request.BikeId}!",
                CreatedDate = DateTime.Now
            };
            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();

            return Ok(rental);
        }

        // GET: api/rental
        [HttpGet]
        public async Task<IActionResult> GetAllRentals()
        {
            var rentals = await _context.Rentals
            .Select(r => new {
                r.RentalId,
                r.UserId,
                r.BikeId,
                r.StartTime,
                r.DuedTime,
                r.ReturnedTimme,
                r.Status,
                User = new
                {
                    r.User.UserId,
                    r.User.FullName
                },
                Bike = new
                {
                    r.Bike.BikeId,
                    r.Bike.Status,
                    r.Bike.Price
                }
            })
            .ToListAsync();
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
        public async Task<IActionResult> ReturnRental(int id, UpdateRequestRentalDtos updateRequest)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
                return NotFound();

            rental.ReturnedTimme = DateTime.Now;
            rental.Status = updateRequest.RentalStatus;
            var bike = await _context.Bikes.FindAsync(rental.BikeId);   
            bike.Status = updateRequest.BikeStatus; 

            await _context.SaveChangesAsync();

            var notification = new Notifications
            {
                UserId = updateRequest.UserId,
                Message = $"You have successfully returned bike {updateRequest.BikeId}!",
                CreatedDate = DateTime.Now
            };
            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
