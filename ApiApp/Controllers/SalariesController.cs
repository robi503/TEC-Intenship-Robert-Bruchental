using Internship.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Internship.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SalariesController : ControllerBase
    {
        private readonly APIDbContext _context;

        public SalariesController(APIDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Salary>>> Get()
        {
            var list = await _context.Salaries.ToListAsync();
            return Ok(list);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Salary>> Get(int Id)
        {
            Salary? salary = await _context.Salaries.FindAsync(Id);
            if (salary == null)
                return NotFound();
            else
                return Ok(salary);
        }

        [HttpPost]
        public async Task<ActionResult<Salary>> Post(Salary salary)
        {
            if (ModelState.IsValid)
            {
                _context.Salaries.Add(salary);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { salary.SalaryId }, salary);
            }
            else
                return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDepartment(Salary salary)
        {
            if (ModelState.IsValid)
            {
                Salary? updateSalary = await _context.Salaries.FindAsync(salary.SalaryId);
                if (updateSalary == null)
                {
                    return NotFound();
                }
                updateSalary.Amount = salary.Amount;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else
                return BadRequest();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            Salary? salary = await _context.Salaries.FindAsync(Id);
            if (salary == null)
                return NotFound();
            else
            {
                _context.Salaries.Remove(salary);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
