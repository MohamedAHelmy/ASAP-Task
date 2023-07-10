using ASAP_Task.Core;
using ASAP_Task.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASAP_Task.Controllers
{
    [ApiController]
    [Route("api/people")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPeople()
        {
            var people = await _personService.GetAllPeople();
            return Ok(people);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonById(int id)
        {
            var person = await _personService.GetPersonById(id);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }

        [HttpPost]
        public async Task<IActionResult> AddPerson([FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _personService.AddPerson(person);
            return CreatedAtAction(nameof(GetPersonById), new { id = person.Id }, person);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }
            await _personService.UpdatePerson(person);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            await _personService.DeletePerson(id);
            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterPeople(string name, int age)
        {
            var filteredPeople = await _personService.FilterPeople(name, age);
            return Ok(filteredPeople);
        }
    }
}
