using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterProject.Api.Common;
using StarterProject.Api.Dtos.Users;
using StarterProject.Api.Features.Users;
using StarterProject.Api.Features.Users.Dtos;
using StarterProject.Api.Services;
using System.Collections.Generic;
using System.Net;

namespace StarterProject.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public UsersController(
            IUserService userService,
            IUserRepository userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("[controller]/authenticate")]
        [ProducesResponseType(typeof(UserAuthenticationGetDto), (int) HttpStatusCode.OK)]
        public IActionResult Authenticate([FromBody] UserAuthenticationDto userAuthenticationDto)
        {
            var userAuth = _userService.Authenticate(userAuthenticationDto);

            if (userAuth == null)
                return BadRequest(new {message = "Username or password is incorrect"});

            return Ok(userAuth);
        }
        
        [HttpGet("[controller]")]
        [ProducesResponseType(typeof(List<UserGetDto>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll()
        {
            var users = _userRepository.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("[controller]/{userId:int}")]
        [ProducesResponseType(typeof(UserGetDto), (int)HttpStatusCode.OK)]
        public IActionResult Get(int userId)
        {
            var user = _userRepository.GetUser(userId);
            return Ok(user);
        }
        
        [HttpPost("[controller]")]
        [ProducesResponseType(typeof(UserGetDto), (int)HttpStatusCode.Created)]
        public IActionResult Post([FromBody] UserCreateDto userCreateDto)
        {
            var user = _userRepository.CreateUser(userCreateDto);
            return Created("[controller]", user);
        }

        [HttpPut("[controller]/{userId:int}")]
        [ProducesResponseType(typeof(UserGetDto), (int)HttpStatusCode.OK)]
        public IActionResult Put(int userId, [FromBody] UserEditDto userEditDto)
        {
            var user = _userRepository.EditUser(userId, userEditDto);
            return Ok(user);
        }

        //[Authorize(Roles = Constants.Users.Roles.Admin)]
        [Authorize(Policy = Constants.Policies.CanChangeUserRole)]
        [HttpPut("[controller]/{userId:int}/Role")]
        [ProducesResponseType(typeof(UserGetDto), (int)HttpStatusCode.OK)]
        public IActionResult PutRole(int userId, [FromBody] UserRoleEditDto userRoleEditDto)
        {
            var user = _userRepository.EditUserRole(userId, userRoleEditDto);
            return Ok(user);
        }

        [HttpDelete("[controller]/{userId:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Delete(int userId)
        {
            _userRepository.DeleteUser(userId);
            return Ok();
        }
    }
}
