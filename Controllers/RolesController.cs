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
    public class RolesController : ControllerBase
    {
        private readonly CSW306DBContext _context;

        public RolesController(CSW306DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<Role>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRoleById(int id )
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId  == id);
            if (role == null)  return NotFound("Role not found");
            return Ok(role);
                    
        }
        [HttpPost]
        public async Task <ActionResult <Role>> CreateRole(RoleDto roleDto)
        {
            try
            {
                var role =  new Role
                {
                    RoleName = roleDto.RoleName,
                    CreatedDate = DateTime.Now,
                };
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();
                return Ok(role); 

            }
            catch (Exception ex) { 
                 return BadRequest(ex.Message);
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Role>> UpdateRole(int id, RoleDto rolePutDto)
        {

            try
            {
                if (rolePutDto == null)
                {
                    return BadRequest("Request body cannot be null.");
                }

                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                {
                    return NotFound($"Role with ID {id} not found.");
                }

                role.RoleName = rolePutDto.RoleName;
                await _context.SaveChangesAsync();
                return Ok(role);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound("Role not found.");

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return Ok("Deleted");
        }

    }
}
