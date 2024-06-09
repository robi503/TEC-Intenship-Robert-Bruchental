using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiApp.Model;
using Internship.Model;


namespace ApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController(APIDbContext context, JwtService jwtService) : ControllerBase
    {
        private readonly APIDbContext _context = context;
        private readonly JwtService _jwtService = jwtService;

        [HttpGet("{username}/{password}")]
        public async Task<IActionResult> GetAdmin(string username, string password)
        {
            try
            {
                // This line does not work with await I don't know why
                var admin =  _context.Admins.FirstOrDefault(a => a.Username == username && a.Password == password);
                if (admin == null)
                {
                    return NotFound();
                }

                string token = _jwtService.GenerateToken(username);

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        public async Task<ActionResult<Admin>> PostAdmin(Admin admin)
        {
            try
            {
                _context.Admins.Add(admin);
                await _context.SaveChangesAsync();
                return Ok(admin);
            }
            catch (DbUpdateException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
