using ASAP_Task.Core;
using ASAP_Task.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace ASAP_Task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [Authorize]
        [HttpGet("GetAllPeople")]
        public async Task<IActionResult> GetAllPeople()
        {
            var people = await _personService.GetAllPeople();
            return Ok(people);
        }

        [Authorize]
        [HttpGet("GetPerson/{id}")]
        public async Task<IActionResult> GetPersonById(int id)
        {
            var person = await _personService.GetPersonById(id);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddPerson")]
        public async Task<IActionResult> AddPerson([FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _personService.AddPerson(person);
            return CreatedAtAction(nameof(GetPersonById), new { id = person.Id }, person);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdatePerson/{id}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }
            await _personService.UpdatePerson(person);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeletePerson/{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            await _personService.DeletePerson(id);
            return NoContent();
        }

        [Authorize]
        [HttpGet("filter")]
        public async Task<IActionResult> FilterPeople(string name, int age)
        {
            var filteredPeople = await _personService.FilterPeople(name, age);
            return Ok(filteredPeople);
        }
    }
}
