
using APIClient;
using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly RestaurantApiClient _restaurantApiClient;

        public RestaurantController(RestaurantApiClient restaurantApiClient)
        {
            _restaurantApiClient = restaurantApiClient;
        }

        public IActionResult Index(int id)
        {
            return Content($"RestaurantController reached. ID: {id}");
        }
    }
}
