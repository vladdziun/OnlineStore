using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.Api.Entities;
using OnlineStore.Api.Extensions;
using OnlineStore.Api.Repositories;
using OnlineStore.Api.Repositories.Contracts;
using OnlineStore.Models.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginRegController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        public LoginRegController(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        /// <summary>
        /// Register a user.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UserDto>> Register([FromBody] UserDto userToRegister)
        {
            try
            {
                var newUser = await _userRepository.RegisterUser(userToRegister);

                if (newUser == null)
                {
                    throw new Exception($"Something went wrong");
                }

                var newUserDto = newUser.ConvertToDto();
                return Ok(newUserDto);


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// Logins a user.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] UserDto userLogin)
        {
            var user = await Authenticate(userLogin);

            if (user != null)
            {
                var token = Generate(user);
                HttpContext.Session.SetString("jwt_token", token);
                return Ok(token);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<User> Authenticate(UserDto userLogin)
        {
            var currentUser = await _userRepository.GetUser(userLogin.UserName);
            PasswordVerificationResult res;
            if (currentUser == null)
            {
                return null;
            }
            else
            {
                PasswordHasher<UserDto> Hasher = new PasswordHasher<UserDto>();
                res = Hasher.VerifyHashedPassword(userLogin, currentUser.PasswordHash, userLogin.PasswordHash);
            }


            if (res == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return currentUser;
        }
    }
}
