using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Api.Entities;
using OnlineStore.Api.Repositories;
using OnlineStore.Api.Repositories.Contracts;
using OnlineStore.Models.Dtos;

namespace OnlineStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginRegController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public LoginRegController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Register a user.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CartItemDto>> Register([FromBody] User userToRegister)
        {
            try
            {
                var newCartItem = await _userRepository.RegisterUser(userToRegister);

                if (newCartItem == null)
                {
                    throw new Exception($"Something went wrong");
                }

                return CreatedAtAction(nameof(Register), new { id = userToRegister.Id }, userToRegister);


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
