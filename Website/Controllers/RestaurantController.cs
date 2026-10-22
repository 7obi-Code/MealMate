using APIClient;
using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers;

public class RestaurantController : Controller
{
    private readonly RestaurantApiClient _restaurantApiClient;

    public RestaurantController(RestaurantApiClient restaurantApiClient)
    {
        _restaurantApiClient = restaurantApiClient;
    }

    [HttpGet]
    public IActionResult Index(int id)
    {
        var restaurant = _restaurantApiClient.GetRestaurantMenuById(id);
        if (restaurant == null)
        {
            return NotFound();
        }

        return View(restaurant);
    }
}
