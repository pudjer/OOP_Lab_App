using AT_API.Utilities;
using AT_Domain.DTOs.InDTOs;
using AT_Domain.DTOs.OutDTOs;
using AT_Domain.Models;
using AT_Infrastructure.Facades;
using AT_Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace AT_API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAuthenticationFacade _authenticationFacade;
        private readonly IBaseModelRepository<User> _userRepository;

        public UsersController(IAuthenticationFacade authenticationFacade,
            IBaseModelRepository<User> userRepository)
        {
            _authenticationFacade = authenticationFacade;
            _userRepository = userRepository;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> Me()
        {
            var current_user = await this.GetUserAsync();
            return Ok(new UserDTO(current_user!));
        }

        [HttpPost("register")]
        public async Task<ActionResult<SignedUpDTO>> Register(RegisterDTO inDto)
        {
            // i kinda forgot that it can actually do most of the validation
            // even before reaching the controller
            string username = inDto.Username;
            string password = inDto.Password;
            var outDto = await _authenticationFacade.Register(username, password);

            // this could be literally anything from invalid username or password
            // to signing up with a username that already exists
            // so will need to specify a little bit on this later
            if (outDto == null)
            {
                return BadRequest("User with that username already exists.");
            }

            // actually I even doubt whether the facade actually needs to return a DTO
            // and not something else
            return outDto;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoggedInDTO>> Login(LoginDTO inDto)
        {
            string username = inDto.Username;
            string password = inDto.Password;
            var outDto = await _authenticationFacade.Authenticate(username, password);

            if (outDto == null)
            {
                return BadRequest("Invalid login credentials.");
            }

            return outDto;
        }

        [HttpPost("subscribe")]
        [Authorize]
        public async Task<ActionResult> Subscribe()
        {
            var current_user = await this.GetUserAsync();
            if (current_user == null)
            {
                return NotFound();
            }
            if (current_user.IsSubscribed)
            { 
              return NoContent();
            }
            current_user.IsSubscribed = true;
            current_user = await _userRepository.UpdateAsync(current_user);
            return Ok(new UserDTO(current_user!));
        }

        [HttpPost("unsubscribe")]
        [Authorize]
        public async Task<ActionResult> Unsubscribe()
        {
            var user = await this.GetUserAsync();
            if (user == null)
            {
                return NotFound();
            }
            if (!user.IsSubscribed)
            {
                return NoContent();
            }
            user.IsSubscribed = false;
            user = await _userRepository.UpdateAsync(user);
            return Ok(new UserDTO(user!));
        }
    }
}
