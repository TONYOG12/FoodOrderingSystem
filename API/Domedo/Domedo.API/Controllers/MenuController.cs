using Domedo.App.IRepositroy;
using Domedo.Domain.Requests.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Domedo.API.Controllers
{
    [Route("api/v{version:apiVersion}/menu")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuRepository _repo;

        public MenuController(IMenuRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetMenus([FromQuery(Name = "page")] int page = 1,
          [FromQuery(Name = "pageSize")] int pageSize = 5,
          [FromQuery(Name = "searchQuery")] string searchQuery = null,
          [FromQuery(Name = "with-disabled")] bool withDisabled = false)
        {
            var userId = (string)HttpContext.Items["Sub"];
            if (!ModelState.IsValid || userId == null)
                return UnprocessableEntity(ModelState);

            return Ok(await _repo.GetMenus(page, pageSize, searchQuery, Guid.Parse(userId), withDisabled));

        }


        [HttpGet("with-meals")]
        public async Task<IActionResult> GetMealMenus([FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "pageSize")] int pageSize = 5,
            [FromQuery(Name = "searchQuery")] string searchQuery = null,
            [FromQuery(Name = "with-disabled")] bool withDisabled = false)
        {
           
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            return Ok(await _repo.GetMealMenus(page, pageSize, searchQuery, withDisabled));

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenu(Guid id)
        {
            return Ok(await _repo.GetMenu(id));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateMenu([FromBody] CreateMenuRequest request)
        {
            var userId = (string)HttpContext.Items["Sub"];
            if (!ModelState.IsValid || userId == null)
                return UnprocessableEntity(ModelState);


            return Created(string.Empty, await _repo.CreateMenu(request, Guid.Parse(userId)));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenu([FromBody] UpdateMenuRequest request, Guid id)
        {
            var userId = (string)HttpContext.Items["Sub"];
            if (!ModelState.IsValid || userId == null)
                return UnprocessableEntity(ModelState);

            await _repo.UpdateMenu(request, id, Guid.Parse(userId));
            return NoContent();

        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(Guid id)
        {
            var userId = (string)HttpContext.Items["Sub"];
            if (!ModelState.IsValid || userId == null)
                return UnprocessableEntity(ModelState);

            await _repo.DeleteMenu(id, Guid.Parse(userId));

            return NoContent();
        }
    }
}
