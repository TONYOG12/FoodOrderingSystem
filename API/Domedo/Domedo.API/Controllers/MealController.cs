using Domedo.App.IRepositroy;
using Domedo.Domain.Requests.Meal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Domedo.API.Controllers
{
    [Route("api/v{version:apiVersion}/meal")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private readonly IMealRepository _repo;

        public MealController(IMealRepository repo)
        {
            _repo = repo;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateMeal([FromBody] CreateMealRequest request)
        {
            var userId = (string)HttpContext.Items["Sub"];
            if (!ModelState.IsValid || userId == null)
                return UnprocessableEntity(ModelState);

            try
            {
                return Created(string.Empty, await _repo.CreateMeal(request, Guid.Parse(userId)));

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetMeals([FromQuery(Name = "page")] int page = 1,
           [FromQuery(Name = "pageSize")] int pageSize = 5,
           [FromQuery(Name = "searchQuery")] string searchQuery = null,
           [FromQuery(Name = "with-disabled")] bool withDisabled = false)
        {
           
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            return Ok(await _repo.GetMeals(page, pageSize, searchQuery, withDisabled));

        }

        [HttpGet("by-menu/{menuId}")]
        public async Task<IActionResult> GetMealsByMenuId([FromRoute] Guid menuId, [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "pageSize")] int pageSize = 5,
            [FromQuery(Name = "with-disabled")] bool withDisabled = false)
        {
            //var userId = (string)HttpContext.Items["Sub"];
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            return Ok(await _repo.GetMealsByMenuId(page, pageSize, menuId, withDisabled));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMeal(Guid id)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            return Ok(await _repo.GetMeal(id));
        }
       

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeal([FromBody] UpdateMealRequest request, Guid id)
        {
            var userId = (string)HttpContext.Items["Sub"];
            if (!ModelState.IsValid || userId == null)
                return UnprocessableEntity(ModelState);

            await _repo.UpdateMeal(request, id, Guid.Parse(userId));
            return NoContent();

        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeal(Guid id)
        {
            var userId = (string)HttpContext.Items["Sub"];
            if (!ModelState.IsValid || userId == null)
                return UnprocessableEntity(ModelState);

            await _repo.DeleteMeal(id, Guid.Parse(userId));

            return NoContent();
        }
    }
}
