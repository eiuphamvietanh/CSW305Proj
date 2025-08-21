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
    public class BikesController : ControllerBase
    {
        private readonly CSW306DBContext _context;

        public BikesController(CSW306DBContext context) { _context = context; }

        [HttpGet]
        public async Task<List<BikeDtoResult>> GetAllBikes() {
            var bikes = await _context.Bikes.Include(b => b.BikeStation).Include(b => b.BikeCategory).Select(
                b => new BikeDtoResult
                {
                    BikeId = b.BikeId,
                    BikeCode = b.BikeCode,
                    BikeName = b.BikeName,
                    Model = b.Model,
                    Status = b.Status,
                    CreatedDate = b.CreatedDate,
                    UpdatedDate = b.UpdatedDate,
                    BikeCategory = new BikeCategoryDtos
                    {
                        BikeCategoryName = b.BikeCategory.BikeCategoryName,
                        Description = b.BikeCategory.Description,
                        IsActived = b.BikeCategory.IsActived
                    },
                    BikeStation = new BikeStationDtos
                    {
                        StationName = b.BikeStation.StationName,
                        Location = b.BikeStation.Location,
                        Capacity = b.BikeStation.Capacity, 
                        IsActived = b.BikeStation.IsActived,
                    }

                }

                ).ToListAsync();
            return bikes;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Bike>> GetBikeById(int id)
        {
            var bike = await _context.Bikes
                    .Include(b => b.BikeStation)
                    .Include(b => b.BikeCategory)
                    .FirstOrDefaultAsync(b => b.BikeId == id);
            if (bike == null) {
                return Ok($"Bike with the id {id} do not exist ");
            }
            var bikeDto = new BikeDtoResult
            {
                BikeId = bike.BikeId,
                BikeCode = bike.BikeCode,
                BikeName = bike.BikeName,
                Model = bike.Model,
                Status = bike.Status,
                CreatedDate = bike.CreatedDate,
                UpdatedDate = bike.UpdatedDate,
                BikeCategory = new BikeCategoryDtos
                {
                    BikeCategoryName = bike.BikeCategory.BikeCategoryName,
                    Description = bike.BikeCategory.Description,
                    IsActived = bike.BikeCategory.IsActived
                },
                BikeStation = new BikeStationDtos
                {
                    StationName = bike.BikeStation.StationName,
                    Location = bike.BikeStation.Location,
                    Capacity = bike.BikeStation.Capacity,
                    IsActived = bike.BikeStation.IsActived
                }
            };

            return Ok(bikeDto);
          
        }
        [HttpPost]
        public async Task<ActionResult<BikeDtos>> CreateBike (BikeDtos createdBikeDtos)
        {
            if (createdBikeDtos == null) return BadRequest("Cannot be null");
            var codeExist = await GetBikeByCodeAsync(createdBikeDtos.BikeCode);
            if (codeExist != null)
            {
                return BadRequest("THe code of the bike existed");
            }
            var bike = new Bike
            {
                BikeCode = createdBikeDtos.BikeCode,
                BikeName = createdBikeDtos.BikeName,
                Model = createdBikeDtos.Model,
                Status = createdBikeDtos.Status,
                Price = createdBikeDtos.Price,
                BikeStationId = createdBikeDtos.BikeStationId,
                BikeCategoryId = createdBikeDtos.BikeCategoryId,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };
            _context.Bikes.Add(bike);
            await _context.SaveChangesAsync();
            var bikeDtos = new BikeDtos
            {
                BikeCode = bike.BikeCode,
                BikeName = bike.BikeName,
                Model = bike.Model,
                Status = bike.Status,
                Price = bike.Price,
                BikeCategoryId = bike.BikeCategoryId,
                BikeStationId = bike.BikeStationId,

            };
          
            return Ok(bikeDtos);


        }
        [HttpPut("{id}")]
        public async Task<ActionResult<BikeDtos>> UpdatedBike (int id, BikeDtos updatedBikeDtos)
        {
            if(updatedBikeDtos == null)  return BadRequest("can not be null");
            var bike =  await _context.Bikes.FirstOrDefaultAsync(b => b.BikeId == id);
         
            if (bike == null) return NotFound("The id does not exist");
            var codeExist = await GetBikeByCodeAsync(updatedBikeDtos.BikeCode);
            if (codeExist != null) return BadRequest("The code existed");
            bike.BikeCode = updatedBikeDtos.BikeCode;   
            bike.BikeName = updatedBikeDtos.BikeName;
            bike.Model = updatedBikeDtos.Model;
            bike.Status = updatedBikeDtos.Status;
            bike.Price = updatedBikeDtos.Price;
            bike.BikeStationId = updatedBikeDtos.BikeStationId;
            bike.BikeCategoryId =  updatedBikeDtos.BikeCategoryId;
            bike.UpdatedDate  = DateTime.Now;
            await _context.SaveChangesAsync();

            var bikeDtos = new BikeDtos
            {
                BikeCode = bike.BikeCode,
                BikeName = bike.BikeName,
                Model = bike.Model,
                Status = bike.Status,
                Price = bike.Price,
                BikeCategoryId = bike.BikeCategoryId,
                BikeStationId = bike.BikeStationId,

            };

            return Ok(bikeDtos);
          
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Bike>> DeleteBikeById (int id)
        {
            var bike = await _context.Bikes.FirstOrDefaultAsync( b => b.BikeId == id);
            if (bike == null) return NotFound("CANNOT FOUND ID");
            _context.Bikes.Remove(bike);
            await _context.SaveChangesAsync();
            return Ok("Deleted");
        }
        private async Task<Bike?> GetBikeByCodeAsync(string code)
        {
            return await _context.Bikes.FirstOrDefaultAsync(b => b.BikeCode == code);
        }


    }
}
