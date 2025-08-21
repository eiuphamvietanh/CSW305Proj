using CSW305Proj.Data;
using CSW305Proj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class DiscountsController : ControllerBase
{
    private readonly CSW306DBContext _context;

    public DiscountsController(CSW306DBContext context)
    {
        _context = context;
    }

  
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Discount>>> GetDiscounts()
    {
       
        return await _context.Discounts.ToListAsync();
    }

    
    [HttpGet("{id}")]
    public async Task<ActionResult<Discount>> GetDiscount(int id)
    {
        
        var discount = await _context.Discounts.FindAsync(id);

        if (discount == null)
        {
            return NotFound();
        }

        return discount; 
    }

   
    [HttpPost]
    public async Task<ActionResult<Discount>> PostDiscount(Discount discount)
    {
        
        _context.Discounts.Add(discount);
        await _context.SaveChangesAsync();

       
        return CreatedAtAction(nameof(GetDiscount), new { id = discount.DiscountId }, discount);
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDiscount(int id, Discount discount)
    {
      
        if (id != discount.DiscountId)
        {
            return BadRequest(); 
        }

        _context.Entry(discount).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Discounts.Any(e => e.DiscountId == id))
            {
                return NotFound(); 
            }
            else
            {
                throw;
            }
        }
        return NoContent(); 
    }

   
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDiscount(int id)
    {
        
        var discount = await _context.Discounts.FindAsync(id);
        if (discount == null)
        {
            return NotFound();
        }

        _context.Discounts.Remove(discount);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}