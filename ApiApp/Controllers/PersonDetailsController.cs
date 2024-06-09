using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiApp.Model;
using Internship.Model;
using ApiApp.ObjectModel;
using Microsoft.AspNetCore.Authorization;

namespace ApiApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PersonDetailsController(APIDbContext context) : ControllerBase
    {
        private readonly APIDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDetailsInformation>>> GetPersonsDetails()
        {
            var list = await _context.PersonsDetails
                .Include(x => x.Person)
                .Select(x => new PersonDetailsInformation()
                {
                    Id = x.Id,
                    BirthDay = x.BirthDay,
                    PersonCity = x.PersonCity,
                    PersonName = x.Person.Name
                }).ToListAsync();

            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDetails>> GetPersonDetails(int id)
        {
            var personDetails = await _context.PersonsDetails.FindAsync(id);

            if (personDetails == null)
            {
                return NotFound();
            }

            return Ok(personDetails);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePersonDetails(PersonDetailsCreate personDetails)
        {
            if (ModelState.IsValid)
            {
                PersonDetails? updatePersonDetails = _context.PersonsDetails.Find(personDetails.Id);
                if (updatePersonDetails == null)
                {
                    return NotFound();
                }
                updatePersonDetails.BirthDay = personDetails.BirthDay;
                updatePersonDetails.PersonCity = personDetails.PersonCity;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else
                return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<PersonDetails>> AddPersonDetails(PersonDetailsCreate personDetails)
        {
            if (ModelState.IsValid)
            {
                // Check if a PersonDetails record with the specified PersonId already exists
                PersonDetails? existingPersonDetails = _context.PersonsDetails.SingleOrDefault(pd => pd.PersonId == personDetails.PersonId);

                if (existingPersonDetails != null)
                {
                    // If PersonDetails already exists, update its properties
                    existingPersonDetails.BirthDay = personDetails.BirthDay;
                    existingPersonDetails.PersonCity = personDetails.PersonCity;
                    _context.PersonsDetails.Update(existingPersonDetails);
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                else
                {
                    // If PersonDetails doesn't exist, create a new one
                    Person? person = _context.Persons.Find(personDetails.PersonId);
                    if (person == null)
                    {
                        return NotFound();
                    }
                    PersonDetails newPersonDetails = new PersonDetails
                    {
                        BirthDay = personDetails.BirthDay,
                        PersonCity = personDetails.PersonCity,
                        Person = person
                    };
                    _context.PersonsDetails.Add(newPersonDetails);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetPersonDetails), new { newPersonDetails.Id }, newPersonDetails);
                }

            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonDetails(int id)
        {
            var personDetails = await _context.PersonsDetails.FindAsync(id);
            if (personDetails == null)
            {
                return NotFound();
            }

            _context.PersonsDetails.Remove(personDetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
