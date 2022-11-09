using Domedo.App.IRepositroy;
using Domedo.Domain.Requests.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Domedo.API.Controllers
{

    [Route("api/v{version:apiVersion}/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _repo;

        public OrderController(IOrderRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            return Created(string.Empty, await _repo.CreateOrder(request));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var userId = (string)HttpContext.Items["Sub"];
            if (!ModelState.IsValid || userId == null)
                return UnprocessableEntity(ModelState);

            try
            {
                return Ok(await _repo.GetOrder(id));

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrders([FromQuery(Name = "page")] int page = 1,
          [FromQuery(Name = "pageSize")] int pageSize = 5,
          [FromQuery(Name = "searchQuery")] string searchQuery = null,
          [FromQuery(Name = "with-disabled")] bool withCompleted = false)
        {
            var userId = (string)HttpContext.Items["Sub"];
            if (!ModelState.IsValid || userId == null)
                return UnprocessableEntity(ModelState);

            return Ok(await _repo.GetOrders(page, pageSize, searchQuery, Guid.Parse(userId), withCompleted));

        }

        [HttpGet("{id}/complete-order")]
        [Authorize]
        public async Task<IActionResult> CompleteOrder(Guid id)
        {
            var userId = (string)HttpContext.Items["Sub"];
            if (!ModelState.IsValid || userId == null)
                return UnprocessableEntity(ModelState);

            await _repo.CompleteOrder(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest request, Guid id)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _repo.UpdateOrder(request, id);
            return NoContent();
        }
    }
}
