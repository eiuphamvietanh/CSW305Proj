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
        public async Task<List<BikeStation>> GetAllStations () {
            return await _context.BikeStations.ToListAsync();
       }
        [HttpGet("{id}")]
        public async Task<ActionResult<BikeStation>> GetStationById(int id)
        {
            var station = await  _context.BikeStations.FirstOrDefaultAsync(s => s.StationId == id);
            if (station == null) { 
                return NotFound("StationId Not Found");
            }
            return Ok(station);
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
