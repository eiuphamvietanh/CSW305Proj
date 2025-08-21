using CSW305Proj.Data;
using CSW305Proj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSW305Proj.DTOs
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarouselController : ControllerBase
    {
        private readonly CSW306DBContext _context;
        private readonly IWebHostEnvironment _environment;

        public CarouselController(CSW306DBContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/Carousel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carousels>>> GetCarousels()
        {
            return await _context.Carousels
                .Include(c => c.Bike)
                .Include(c => c.BikeCategory)
                .ToListAsync();
        }

        // GET: api/Carousel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Carousels>> GetCarousel(int id)
        {
            var carousel = await _context.Carousels
                .Include(c => c.Bike)
                .Include(c => c.BikeCategory)
                .FirstOrDefaultAsync(c => c.CarouselId == id);

            if (carousel == null)
            {
                return NotFound();
            }

            return carousel;
        }

        // POST: api/Carousel
        [HttpPost]
        public async Task<ActionResult<Carousels>> CreateCarousel([FromBody] Carousels carousel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (carousel.BikeId.HasValue && !await _context.Bikes.AnyAsync(b => b.BikeId == carousel.BikeId))
            {
                return BadRequest("Invalid BikeId.");
            }

            if (carousel.BikeCategoryId.HasValue && !await _context.BikeCategories.AnyAsync(bc => bc.BikeCategoryId == carousel.BikeCategoryId))
            {
                return BadRequest("Invalid BikeCategoryId.");
            }

            _context.Carousels.Add(carousel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCarousel), new { id = carousel.CarouselId }, carousel);
        }

        // PUT: api/Carousel/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCarousel(int id, [FromForm] CarouselDTOs carouselDto, IFormFile? image)
        {
            var carousel = await _context.Carousels.FindAsync(id);
            if (carousel == null)
            {
                return NotFound();
            }



            // Validate BikeId if provided
            if (carouselDto.BikeId.HasValue && !await _context.Bikes.AnyAsync(b => b.BikeId == carouselDto.BikeId))
            {
                return BadRequest("Invalid BikeId.");
            }

            // Validate BikeCategoryId if provided
            if (carouselDto.BikeCategoryId.HasValue && !await _context.BikeCategories.AnyAsync(bc => bc.BikeCategoryId == carouselDto.BikeCategoryId))
            {
                return BadRequest("Invalid BikeCategoryId.");
            }

            // Update image if provided
            if (image != null && image.Length > 0)
            {
                // Delete old image if it exists
                if (!string.IsNullOrEmpty(carousel.ImageUrl))
                {
                    var oldFilePath = Path.Combine(_environment.WebRootPath, carousel.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Save new image
                var fileName = $"{Guid.NewGuid()}_{image.FileName}";
                var filePath = Path.Combine(_environment.WebRootPath, "carousel-images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                carousel.ImageUrl = $"/carousel-images/{fileName}";
            }

            carousel.BikeId = carouselDto.BikeId;
            carousel.BikeCategoryId = carouselDto.BikeCategoryId;
            carousel.UpdatedDate = DateTime.Now;

            _context.Entry(carousel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Carousels.AnyAsync(c => c.CarouselId == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        //[HttpDeleteAttribute("{id}")]
        //public ActionResult<Notifications> DeleteCarousel(int id)
        //{
        //    var carousel = _context.Carousels.Find(id);
        //    if (carousel == null)
        //        return Ok(new Notifications(false, $"carousel with ID {id} not found"));

        //    _context.Carousels.Remove(carousel);
        //    _context.SaveChanges();

        //    return Ok(new
        //    {
        //        status = new Notifications(true, "delete successful"),
        //        data = _context.Carousels
        //    });
        //}
    }

}



