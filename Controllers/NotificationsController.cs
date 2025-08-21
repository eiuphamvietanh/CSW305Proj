using CSW305Proj.Data;
using CSW305Proj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSW305Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly CSW306DBContext _context;

        public NotificationsController(CSW306DBContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notifications>>> GetNotifications()
        {
            return await _context.Notifications.ToListAsync();
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<Notifications>> GetNotification(int id)
        {
            var notification = await _context.Notifications.Include(n => n.User)
                                                           .FirstOrDefaultAsync(n => n.NotificationId == id);

            if (notification == null)
            {
                return NotFound();
            }

            return notification;
        }

       
        [HttpPost]
        public async Task<ActionResult<Notifications>> CreateNotification(Notifications notification)
        {
            notification.CreatedDate = DateTime.UtcNow;
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNotification), new { id = notification.NotificationId }, notification);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(int id, Notifications notification)
        {
            if (id != notification.NotificationId)
            {
                return BadRequest();
            }

            _context.Entry(notification).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

     
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NotificationExists(int id)
        {
            return _context.Notifications.Any(e => e.NotificationId == id);
        }
    }
}
