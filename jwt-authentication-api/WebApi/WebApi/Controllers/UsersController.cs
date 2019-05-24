using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebApi.Helpers;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<User>> Authenticate([FromBody]User userParam)
        {
            var user = await _userService
                .Authenticate(userParam.Username, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody]User userParam)
        {
            try
            {
                // save 
                var user = await _userService.Create(userParam, userParam.Password);
                return Ok(user);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
