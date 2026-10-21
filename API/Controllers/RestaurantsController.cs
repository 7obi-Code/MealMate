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
            private readonly RestaurantDao _restaurantDao;

            //API opbygger controller med IConfiguration interfacet, som tager ConnectionString fra appsettings.json
            public RestaurantsController(IConfiguration configuration)
            {
                string? connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("Couldnt find connection string");
                }

                _restaurantDao = new RestaurantDao(connectionString);
            }
        }

        [HttpGet("{restaurantId}")]
        public ActionResult<Restaurant> GetMenuById(int restaurantId)
        {
            try
            {
                var restaurant = _restaurantDao.GetResaurantMenuById(restaurantId);

                if (restaurant == null)
                {
                    return NotFound();
                }

                return Ok(restaurant);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = $"Error retrieving restaurant with ID: {restaurantId}",
                    error = ex.Message
                });
            }
        }
    
}
