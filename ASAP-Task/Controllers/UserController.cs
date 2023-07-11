using ASAP_Task.Core;
using ASAP_Task.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ASAP_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _IUserService;
        public UserController(IUserService IUserService)
        {
            _IUserService = IUserService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel user)
        {
            var Respons = await _IUserService.RegisterAsync(user);
            return Ok(Respons);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var Respons = await _IUserService.LoginAsync(model);
            return Ok(Respons);

        }
    }
}
