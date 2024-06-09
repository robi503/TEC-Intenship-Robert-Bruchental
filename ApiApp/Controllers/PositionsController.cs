using Internship.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ApiApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly APIDbContext _context;

        public PositionsController(APIDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Position>>> Get()
        {
            var list = await _context.Positions.ToListAsync();
            return Ok(list);
        }
    }
}
