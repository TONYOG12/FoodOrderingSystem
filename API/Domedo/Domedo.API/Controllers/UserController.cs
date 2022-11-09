using Domedo.App.IRepositroy;
using Domedo.Domain.Requests.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Domedo.API.Controllers
{

    [Route("api/v{version:apiVersion}/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateRequest model)
        {
            var userId = (string)HttpContext.Items["Sub"];
            if (!ModelState.IsValid || userId == null)
                return UnprocessableEntity(ModelState);

            return Created(string.Empty, await _userRepo.CreateUser(model));

        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            return Ok(await _userRepo.GetUsers());
        }
    }
}
