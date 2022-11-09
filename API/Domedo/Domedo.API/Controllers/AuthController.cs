using Domedo.App.IRepositroy;
using Domedo.Domain.Requests.Auth;
using Domedo.Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Domedo.API.Controllers
{
    [Route("api/v{version:apiVersion}/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;

        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            try
            {
                return Ok(await _authRepo.Login(request));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }


}
