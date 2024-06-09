using Internship.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Internship.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly APIDbContext _context;

        public DepartmentsController(APIDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> Get()
        {
            var list = await _context.Departments.ToListAsync();
            return Ok(list);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Department>> Get(int Id)
        {
            Department? department = await _context.Departments.FindAsync(Id);
            if (department == null)
                return NotFound();
            else
                return Ok(department);
        }

        [HttpPost]
        public async Task<ActionResult<Department>> AddDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { department.DepartmentId }, department);
            }
            else
                return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                Department? updateDepartment = await _context.Departments.FindAsync(department.DepartmentId);
                if (updateDepartment == null)
                {
                    return NotFound();
                }
                updateDepartment.DepartmentName = department.DepartmentName;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else
                return BadRequest();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            Department? department = await _context.Departments.FindAsync(Id);
            if (department == null)
                return NotFound();
            else
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
