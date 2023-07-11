using ASAP_Task.Core;
using ASAP_Task.Service.Implementations;
using ASAP_Task.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ASAP_Task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [Authorize]
        [HttpGet]
        [HttpPost("GetAllAddresses")]
        public async Task<IActionResult> GetAllAddresses()
        {
            var addresses = await _addressService.GetAllAddresses();
            return Ok(addresses);
        }
        [Authorize]
        [HttpGet("GetAddress/{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            var address = await _addressService.GetAddressById(id);
            if (address == null)
            {
                return NotFound();
            }
            return Ok(address);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddAddress")]
        public async Task<IActionResult> AddAddress([FromBody] Address address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _addressService.AddAddress(address);
            return CreatedAtAction(nameof(GetAddressById), new { id = address.Id }, address);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateAddress/{id}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] Address address)
        {
            if (id != address.Id)
            {
                return BadRequest();
            }
            await _addressService.UpdateAddress(address);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteAddress/{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            await _addressService.DeleteAddress(id);
            return NoContent();
        }

        [Authorize]
        [HttpGet("filter")]
        public async Task<IActionResult> FilterAddresses(string Street, string City, string Country)
        {
            var filteredAddresses = await _addressService.FilterAddresses(Street, City, Country);
            return Ok(filteredAddresses);
        }
    }
}
