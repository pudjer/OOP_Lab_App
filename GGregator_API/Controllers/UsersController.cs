using GGregator_Domain.DTOs.InDTOs;
using GGregator_Domain.DTOs.OutDTOs;
using GGregator_Infrastructure.Facades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GGregator_API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IAuthenticationFacade _authenticationFacade;

        public UsersController(DbContext context, IAuthenticationFacade authenticationFacade)
        {
            _context = context;
            _authenticationFacade = authenticationFacade;
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
    }
}
