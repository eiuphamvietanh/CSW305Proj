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
    public class BikeStationsController : ControllerBase
    {
        private readonly CSW306DBContext _context;

        public BikeStationsController (CSW306DBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<List<BikeStationDtoResult>> GetAllStations () {
            var stations = await _context.BikeStations.Include(s => s.Bikes).Select(s => new BikeStationDtoResult
            {
                StationId = s.StationId,
                StationName = s.StationName,
                Location = s.Location,
                Capacity = s.Capacity,
                IsActived = s.IsActived,
                Bikes = s.Bikes.Select(b => new BikeDtoResult
                {
                    BikeId = b.BikeId,
                    BikeCode = b.BikeCode,
                    BikeName = b.BikeName,
                    Model = b.Model,
                    Price = b.Price,
                    Status = b.Status,
                    CreatedDate = b.CreatedDate,
                    UpdatedDate = b.UpdatedDate,
                    BikeCategory = new BikeCategoryDtos
                    {
                        BikeCategoryName = b.BikeCategory.BikeCategoryName,
                        Description = b.BikeCategory.Description,
                        IsActived = b.BikeCategory.IsActived
                    }
                }).ToList()
            }).ToListAsync();
            return stations;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BikeStation>> GetStationById(int id)
        {
            var station = await  _context.BikeStations.FirstOrDefaultAsync(s => s.StationId == id);
            if (station == null) { 
                return NotFound("StationId Not Found");
            }
            return Ok(new {station.StationId, station.Location, station.Capacity});
        }

        [HttpGet("stations/{stationId}/available-bikes")]
        public async Task<IActionResult> GetAvailableBikes(int stationId)
        {
            var station = await _context.BikeStations
                .Include(s => s.Bikes)
                .FirstOrDefaultAsync(s => s.StationId == stationId);

            if (station == null)
                return NotFound("Station not found");

        
              var availableBikes = station.Bikes
                .Where(b => b.Status == "Available")
                .Select(b => new
                {
                    bikeId = b.BikeId,
                    price = b.Price
                })
                .ToList();

             return Ok(new
                    {
                        stationId = station.StationId,
                        stationName = station.StationName,
                        availableBikesCount = availableBikes.Count,
                        bikes = availableBikes
                    });

      
        }
        [HttpPost]
        public async Task <ActionResult<BikeStation>> CreateBikeStation(BikeStationDtos createBikeStationDto)
        {
            if(createBikeStationDto == null)
            {
                return BadRequest("Cannot be null");
            }
            bool nameExists = await _context.BikeStations
             .AnyAsync(bc => bc.StationName.Trim().ToLower() == createBikeStationDto.StationName.Trim().ToLower());

            if (nameExists)
                return Conflict($"Bike category '{createBikeStationDto.StationName}' already exists.");
            var bikeStation = new BikeStation
            {
                StationName = createBikeStationDto.StationName,
                Location = createBikeStationDto.Location,
                Capacity = createBikeStationDto.Capacity,
                IsActived = createBikeStationDto.IsActived,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,

            };
            _context.BikeStations.Add(bikeStation);
            await _context.SaveChangesAsync();
            return Ok(bikeStation);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<BikeStation>> UpdateBikeStation (int id, BikeStationDtos updateBikeStationDto)
        {
            try
            {
                if (updateBikeStationDto == null)
                {
                    return BadRequest("Cannot be null");
                }
                var bikeStation = await _context.BikeStations.FirstOrDefaultAsync( b => b.StationId == id );    
                if( bikeStation == null )
                {
                    return NotFound("StationId not found");
                }
                bikeStation.StationName = updateBikeStationDto.StationName; 
                bikeStation.Location    = updateBikeStationDto.Location;    
                bikeStation.Capacity = updateBikeStationDto.Capacity;
                bikeStation.IsActived = updateBikeStationDto.IsActived;
                bikeStation.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();  
                return Ok(bikeStation);


            }
            catch (Exception ex) {
                   return BadRequest(ex.Message);
            }

        }
        [HttpDelete("{id}")]
        public async Task <ActionResult<BikeStation>> DeleteBikeStation(int id)
        {
            var bikeStation = await _context.BikeStations.FirstOrDefaultAsync ( b => b.StationId == id );
            if (bikeStation == null) return NotFound("BikeStationId do not exist");
            _context.BikeStations.Remove(bikeStation );
            await _context.SaveChangesAsync();  
            return Ok($"Deleted the `{bikeStation.StationName}`");
        }
    }
}
