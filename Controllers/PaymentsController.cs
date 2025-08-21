using CSW305Proj.Data;
using CSW305Proj.DTOs;
using CSW305Proj.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace CSW305Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly CSW306DBContext _context;

        public PaymentsController(CSW306DBContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDtos createPaymentDtos)
        {
            if (createPaymentDtos == null || createPaymentDtos.UserId <= 0)
                return BadRequest("Invalid payment request.");

           
            var rentals = await _context.Rentals
                .Include(r => r.Bike)
                .Where(r => r.UserId == createPaymentDtos.UserId && r.Status == "Returned")
                .ToListAsync();

            if (!rentals.Any())
                return BadRequest("No rentals found with status Returned for this user.");

            decimal totalBeforeAmount = 0;

           
            foreach (var rental in rentals)
            {
                if (rental.ReturnedTimme.HasValue && rental.ReturnedTimme.HasValue && rental.Bike != null)
                {
                    
                    var duration = rental.ReturnedTimme.Value - rental.StartTime;
                    var hours = Math.Max(1, duration.TotalHours); 

                  
                    decimal rentalCost = (decimal)hours * rental.Bike.Price;

                    totalBeforeAmount += rentalCost;
                }

                rental.Status = "Paid"; 
            }
            var BeforeAmount = totalBeforeAmount;

            decimal afterAmount = totalBeforeAmount;
            if (createPaymentDtos.DiscountId > 0)
            {
                var discount = await _context.Discounts.FindAsync(createPaymentDtos.DiscountId);
                if (discount != null && discount.IsActive && DateTime.UtcNow >= discount.StartDate && DateTime.UtcNow <= discount.EndDate)
                {
                    if (discount.DiscountType == 1)
                    {
                        afterAmount = totalBeforeAmount * (1 - discount.Value / 100);
                    }
                    else if (discount.DiscountType == 2)
                    {
                        afterAmount = Math.Max(0, totalBeforeAmount - discount.Value);
                    }
                }
            }
             var payment = new Payment {
                 UserID = createPaymentDtos.UserId,
                 DiscountId = createPaymentDtos.DiscountId,
                 BeforeAmount = totalBeforeAmount,
                 AfterAmount = afterAmount,
                 Method = createPaymentDtos.Method,
                 Note = createPaymentDtos.Note,
                 CreatedDate = DateTime.UtcNow,
             };   


            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Payment created, rentals updated to Paid.",
                Payment = payment,
                Rentals = rentals.Select(r => new
                {
                    r.RentalId,
                    r.Status,
                    r.StartTime,
                    r.ReturnedTimme,
                    Bike = r.Bike?.Model,
                    PricePerHour = r.Bike?.Price,
                    DurationHours = (r.ReturnedTimme.HasValue) ? (r.ReturnedTimme.Value - r.StartTime).TotalHours : 0
                }),
                TotalBeforeAmount = totalBeforeAmount,
                TotalAfterAmount = afterAmount
            });
        }
    }
}