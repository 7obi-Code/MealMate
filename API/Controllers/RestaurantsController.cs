using DataAccess.DAO;
using DataAccess.Model;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RestaurantsController : ControllerBase
    {
        private readonly RestaurantDao _restaurantDao = new();

        [HttpGet("{restaurantId}")]
        public ActionResult<Restaurant> GetMenuById(int restaurantId)
        {
            var restaurant =
                _restaurantDao.GetRestaurantMenuById(restaurantId);

            if (restaurant == null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }
    }
}
