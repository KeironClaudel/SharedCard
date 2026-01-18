using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedCard.Api.Data;
using SharedCard.Api.Models;

namespace SharedCard.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly SharedCardDBContext _context;

        public PeopleController(SharedCardDBContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var people = await _context.People
            .Where(p => p.Active)
            .OrderBy(p => p.Name)
            .ToListAsync();

            return Ok(people);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Person person)
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = person.PersonId }, person);
        }
    }
}
