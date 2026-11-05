
using DataAccess.DTOs;
using DataAccess.Interfaces;
using DataAccess.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FoodItemsController : ControllerBase
    {
        private readonly IFFoodItemDao _foodItemDao;

        public FoodItemsController(IFFoodItemDao foodItemDao)
        {
            _foodItemDao = foodItemDao;
        }

        // GET: api/v1/FoodItems/5
        [HttpGet("{foodItemId}")]
        public ActionResult<FoodItem> GetFoodItemById(int foodItemId)
        {
            var foodItem = _foodItemDao.GetFoodItemById(foodItemId);

            if (foodItem == null)
            {
                return NotFound();
            }

            return Ok(foodItem);
        }

        // POST: api/v1/FoodItems
        [HttpPost]
        public ActionResult<int> CreateFoodItem(
            [FromBody] CreateFoodItemRequest request)
        {
            if (request.CategoryId <= 0 ||
                string.IsNullOrWhiteSpace(request.Name) ||
                request.Amount < 0)
            {
                return BadRequest("Invalid food item information.");
            }

            var foodItem = new FoodItem
            {
                Name = request.Name,
                Description = request.Description ?? string.Empty,
                IsAvailable = request.IsAvailable
            };

            int foodItemId = _foodItemDao.CreateFoodItem(
                foodItem,
                request.CategoryId,
                request.Amount
            );

            return CreatedAtAction(
                nameof(GetFoodItemById),
                new { foodItemId },
                foodItemId
            );
        }

        // PUT: api/v1/FoodItems/5
        [HttpPut("{foodItemId}")]
        public IActionResult UpdateFoodItem(
            int foodItemId,
            [FromBody] UpdateFoodItemRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) ||
                request.Amount < 0)
            {
                return BadRequest("Invalid food item information.");
            }

            var foodItem = new FoodItem
            {
                FoodItemId = foodItemId,
                Name = request.Name,
                Description = request.Description ?? string.Empty,
                IsAvailable = request.IsAvailable
            };

            bool updated = _foodItemDao.UpdateFoodItem(
                foodItem,
                request.Amount
            );

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/v1/FoodItems/5
        [HttpDelete("{foodItemId}")]
        public IActionResult DeleteFoodItem(int foodItemId)
        {
            bool deleted = _foodItemDao.DeleteFoodItem(foodItemId);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
