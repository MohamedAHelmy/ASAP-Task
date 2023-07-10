using ASAP_Task.Core;
using ASAP_Task.Service.Implementations;
using ASAP_Task.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.IO;

namespace ASAP_Task.Controllers
{
    [ApiController]
    [Route("api/addresses")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAddresses()
        {
            var addresses = await _addressService.GetAllAddresses();
            return Ok(addresses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            var address = await _addressService.GetAddressById(id);
            if (address == null)
            {
                return NotFound();
            }
            return Ok(address);
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress([FromBody] Address address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _addressService.AddAddress(address);
            return CreatedAtAction(nameof(GetAddressById), new { id = address.Id }, address);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] Address address)
        {
            if (id != address.Id)
            {
                return BadRequest();
            }
            await _addressService.UpdateAddress(address);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            await _addressService.DeleteAddress(id);
            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterAddresses(string Street, string City, string Country)
        {
            var filteredAddresses = await _addressService.FilterAddresses(Street, City, Country);
            return Ok(filteredAddresses);
        }
    }
}
