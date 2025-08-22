using CSW305Proj.Data;
using CSW305Proj.DTOs;
using CSW305Proj.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSW305Proj.DTOs
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikeCategoriesController : ControllerBase
    {
        private readonly CSW306DBContext _context;
        
        public BikeCategoriesController (CSW306DBContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Authorize]
        public async Task <List<BikeCategoryDtosResult>> GetAllCategories()
        {

            var categories = await _context.BikeCategories.Include(c => c.Bikes).Select(c => new BikeCategoryDtosResult
            {

                BikeCategoryId = c.BikeCategoryId,
                BikeCategoryName = c.BikeCategoryName,
                
                Bikes = c.Bikes.Select(b => new BikeDtos
                {
                    BikeCode = b.BikeCode,
                    BikeName = b.BikeName,
                    Model = b.Model,
                    Status = b.Status,
                    Price = b.Price,
                    BikeCategoryId = b.BikeCategoryId,
                    BikeStationId = b.BikeStationId,

                }).ToList()

            }).ToListAsync();
            return categories;
            
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<BikeCategoryDtosResult>> GetBikeById(int id)
        {
            var bikeCategory = await _context.BikeCategories.FirstOrDefaultAsync(b => b.BikeCategoryId == id);
            if (bikeCategory == null) return Ok("BikeCategoryId not found");
            var bikeDto = new BikeCategoryDtosResult
            {
                BikeCategoryId = bikeCategory.BikeCategoryId,
                BikeCategoryName = bikeCategory.BikeCategoryName,
                Bikes = bikeCategory.Bikes.Select(b => new BikeDtos
                {
                    BikeCode = b.BikeCode,
                    BikeName = b.BikeName,
                    Model = b.Model,
                    Status = b.Status,
                    Price = b.Price,
                    BikeCategoryId = b.BikeCategoryId,
                    BikeStationId = b.BikeStationId
                }).ToList()
            };
            
            return Ok(bikeCategory);    
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<BikeCategory>> CreateBikeCategory(BikeCategoryDtos createBikeCategoryDtos)
        {
            if(createBikeCategoryDtos == null)
            {
                return BadRequest("CAN NOT BE NULL");
            }

            bool nameExists = await _context.BikeCategories
             .AnyAsync(bc => bc.BikeCategoryName.Trim().ToLower() == createBikeCategoryDtos.BikeCategoryName.Trim().ToLower());

            if (nameExists)
                return Conflict($"Bike category '{createBikeCategoryDtos.BikeCategoryName}' already exists.");

            var bikeCategory = new BikeCategory
            {
                BikeCategoryName = createBikeCategoryDtos.BikeCategoryName,
                Description = createBikeCategoryDtos.Description,
                IsActived  = createBikeCategoryDtos.IsActived,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };
            _context.BikeCategories.Add(bikeCategory);
            await _context.SaveChangesAsync();

            return Ok(bikeCategory);

        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Role>> UpdateRole(int id, BikeCategoryDtos bikeCategoryDtos)
        {

            try
            {
                if (bikeCategoryDtos == null)
                {
                    return BadRequest("Request body cannot be null.");
                }

                var bikeCategory = await _context.BikeCategories.FirstOrDefaultAsync(b => b.BikeCategoryId == id);
                if (bikeCategory == null)
                {
                    return NotFound($"Role with ID {id} not found.");
                }
                bikeCategory.UpdatedDate = DateTime.Now;
                bikeCategory.BikeCategoryName = bikeCategoryDtos.BikeCategoryName.Trim();
                bikeCategory.Description = bikeCategoryDtos.Description;    
                bikeCategory.IsActived = bikeCategoryDtos.IsActived;
                
                await _context.SaveChangesAsync();
                return Ok(bikeCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBikeCategory(int id)
        {
            var bikeCategory = await _context.BikeCategories.FindAsync(id);
            if (bikeCategory == null) return NotFound("Role not found.");

            _context.BikeCategories.Remove(bikeCategory);
            await _context.SaveChangesAsync();

            return Ok("Deleted");
        }




    }
}
