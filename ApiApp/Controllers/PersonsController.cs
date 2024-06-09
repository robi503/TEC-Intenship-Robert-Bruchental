using ApiApp.ObjectModel;
using Internship.Model;
using Internship.ObjectModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Internship.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly APIDbContext _context;

        public PersonsController(APIDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonInformation>>> Get()
        {
            var list = await _context.Persons
                .Include(x => x.Salary)
                .Include(x => x.Position)
                    .ThenInclude(p => p.Department)
                .Select(x => new PersonInformation()
                {
                    Id = x.Id,
                    Name = x.Name,
                    PositionName = x.Position.Name,
                    DepartmentName = x.Position.Department.DepartmentName,
                    Salary = x.Salary.Amount,
                }).ToListAsync();

            return Ok(list);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Person>> Get(int Id)
        {
            Person? person = await _context.Persons.FirstOrDefaultAsync(x => x.Id == Id);
            if (person == null)
                return NotFound();
            else
                return Ok(person);
        }

        [HttpPost]
        public async Task<ActionResult<PersonCreate>> Post(PersonCreate person)
        {
            if (ModelState.IsValid)
            {
                Salary? salary = await _context.Salaries.FindAsync(person.SalaryId);
                if (salary == null)
                {
                    return NotFound();
                }
                Position? position = await _context.Positions.FindAsync(person.PositionId);
                if (position == null)
                {
                    return NotFound();
                }
                Person newPerson = new Person
                {
                    Id = person.Id,
                    Name = person.Name,
                    Surname = person.Surname,
                    Address = person.Address,
                    Age = person.Age,
                    Email = person.Email,
                    PositionId = person.PositionId,
                    SalaryId = person.SalaryId,
                    Position = position,
                    Salary = salary
                };

                var personDTO = new PersonCreate
                {
                    Id = person.Id,
                    Name = person.Name,
                    Surname = person.Surname,
                    Age = person.Age,
                    Email = person.Email,
                    Address = person.Address,
                    PositionId = person.PositionId,
                    SalaryId = person.SalaryId
                };
                _context.Persons.Add(newPerson);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { newPerson.Id }, personDTO);
            }
            else
                return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePerson(PersonCreate person)
        {
            if (ModelState.IsValid)
            {
                Person? updatePerson = await _context.Persons.FindAsync(person.Id);
                if (updatePerson == null)
                {
                    return NotFound();
                }
                updatePerson.Address = person.Address;
                updatePerson.Age = person.Age;
                updatePerson.Email = person.Email;
                updatePerson.Name = person.Name;
                updatePerson.PositionId = person.PositionId;
                updatePerson.SalaryId = person.SalaryId;
                updatePerson.Surname = person.Surname;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else
                return BadRequest();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            Person? person = await _context.Persons.FindAsync(Id);
            if (person == null)
                return NotFound();
            else
            {
                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
