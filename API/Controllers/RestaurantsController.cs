
using DataAccess.Interfaces;
using DataAccess.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RestaurantsController : ControllerBase
    {
        private readonly IFRestaurantDao _restaurantDao;

        public RestaurantsController(IFRestaurantDao restaurantDao)
        {
            _restaurantDao = restaurantDao;
        }

        [HttpGet]
        public ActionResult<List<Restaurant>> GetAllRestaurants()
        {
            var restaurants = _restaurantDao.GetAllRestaurants();

            return Ok(restaurants);
        }

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
