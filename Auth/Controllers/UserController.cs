using Notus.Enums;
using Notus.Models.User;
using Notus.Models.User.Dto;
using Notus.Services;
using Notus.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Notus.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AuthServices _authServices;
        private readonly UserServices _userServices;

        public UserController(AuthServices authServices, UserServices userServices)
        {
            _authServices = authServices;
            _userServices = userServices;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status500InternalServerError)]
        async public Task<ActionResult<User>> Register([FromBody]RegisterDTO register)
        {
            try
            {
                var created = await _authServices.Register(register);
                return Created("Register", created);
            }
            catch (HttpResponseError ex)
            {
                return StatusCode(
                    (int)ex.StatusCode,
                    new HttpMessage(ex.Message)
                );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    new HttpMessage(ex.Message)
                );
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status500InternalServerError)]
        async public Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO login)
        {
            try
            {
                var res = await _authServices.Login(login, HttpContext);
                return Ok(res);
            }
            catch (HttpResponseError ex)
            {
                return StatusCode(
                    (int)ex.StatusCode,
                    new HttpMessage(ex.Message)
                );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    new HttpMessage(ex.Message)
                );
            }
        }

        [HttpPut("update")]
        [ProducesResponseType(typeof(UserWithoutPassDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status500InternalServerError)]
        async public Task<ActionResult<UserWithoutPassDTO>> UpdateOneByEmail([FromQuery] string? email, [FromBody] UpdateUserDTO updateDto)
        {
            try
            {
                var user = await _userServices.UpdateOneByEmail(email, updateDto);
                return Ok(user);
            }
            catch (HttpResponseError ex)
            {
                return StatusCode(
                    (int)ex.StatusCode,
                    new HttpMessage(ex.Message)
                );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new HttpMessage(ex.Message)
                );
            }
        }

        [HttpGet("users")]
        async public Task<ActionResult<List<UserWithoutPassDTO>>> GetUsers()
        {
            try
            {
                var users = await _authServices.GetUsers();
                return Ok(users);
            }
            catch (HttpResponseError ex)
            {
                return StatusCode(
                    (int)ex.StatusCode,
                    new HttpMessage(ex.Message)
                );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    new HttpMessage(ex.Message)
                );
            }
        }

        [HttpGet("health")]
        [Authorize(Roles = $"{ROLE.ADMIN}")]
        public bool Health()
        {
            return true;
        }


        [HttpPut("addRole")]
        [Authorize(Roles = $"{ROLE.ADMIN}")]
        [ProducesResponseType(typeof(UserWithoutPassDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status500InternalServerError)]
        async public Task<ActionResult<UserWithoutPassDTO>> AddRole([FromQuery] string? email, [FromBody] string? role)
        {
            try
            {
                var user = await _userServices.AddRoleByEmail(email, role);
                return Ok(user);
            }
            catch (HttpResponseError ex)
            {
                return StatusCode(
                    (int)ex.StatusCode,
                    new HttpMessage(ex.Message)
                );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new HttpMessage(ex.Message)
                );
            }
        }

        [HttpPut("removeRole")]
        [Authorize(Roles = $"{ROLE.ADMIN}")]
        [ProducesResponseType(typeof(UserWithoutPassDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(HttpMessage), StatusCodes.Status500InternalServerError)]
        async public Task<ActionResult<UserWithoutPassDTO>> RemoveRole([FromQuery] string? email, [FromBody] string? role)
        {
            try
            {
                var user = await _userServices.RemoveRoleByEmail(email, role);
                return Ok(user);
            }
            catch (HttpResponseError ex)
            {
                return StatusCode(
                    (int)ex.StatusCode,
                    new HttpMessage(ex.Message)
                );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new HttpMessage(ex.Message)
                );
            }
        }
    }
}
