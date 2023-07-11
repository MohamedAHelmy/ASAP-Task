using ASAP_Task.Core;
using ASAP_Task.Service.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ASAP_Task.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly DataContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;

        public UserService(DataContext dbContext, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }


        public async Task<dynamic> LoginAsync(LoginViewModel model)
        {
            if (model != null)
            {
                var existUser = await _userManager.FindByNameAsync(model.UserName);
                if (existUser == null)
                {

                    return "Email or Password is Invalid";
                }
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);

                    var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "MyCookiie");

                    await _httpContextAccessor.HttpContext.SignInAsync("Identity.Application", new ClaimsPrincipal(claimsIdentity));
                    var token = GenerateJwtToken(user);
                    var response = new LoginResponse
                    {
                        Succeeded = true,
                        Token = token
                    };

                    return response;
                }
                else
                {
                    var response = new LoginResponse
                    {
                        Succeeded = false
                    };

                    return response;

                }
            }
            return "Log in Failed";

        }
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
        public async Task<LoginResponse> RegisterAsync(RegisterViewModel user)
        {
            var response = new LoginResponse();
            if (user == null)
            {
                response.Succeeded = false;
                response.ErrorMessage = new List<IdentityError> { new IdentityError { Description = "No User To Add" } };


                return response;
            }
            if (_dbContext.Users.Any(u => u.Email == user.Email))
            {
                response.Succeeded = false;
                response.ErrorMessage = new List<IdentityError> { new IdentityError { Description = "User with this email already exists." } };
                return response;
            }
            var AppUser = new User
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Name = user.Name,
                isActive = true
            };
            AppUser.NormalizedEmail = _userManager.NormalizeEmail(user.Email);
            var result = await _userManager.CreateAsync(AppUser, user.Password);
            string roleName = user.Role;
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                var role = new IdentityRole(roleName);
                await _roleManager.CreateAsync(role);
            }
            var userdto = new User { UserName = user.Email, Email = user.Email };

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(AppUser, roleName);
                response.Succeeded = true;
                response.ErrorMessage = new List<IdentityError> { new IdentityError { Description = "Registered Successfully" } };
                return response;
            }
            else
            {
                response.ErrorMessage = result.Errors;
                return response;
            }

            response.Succeeded = false;
            response.ErrorMessage = new List<IdentityError> { new IdentityError { Description = "User with this email already exists." } };
            return response;
        }


    }
}
