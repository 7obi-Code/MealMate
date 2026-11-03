using APIClient;
using DataAccess.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers;

[Authorize]
[Route("Manager")]
public class ManagerController : Controller
{
    private readonly RestaurantApiClient _restaurantApiClient;

    public ManagerController(RestaurantApiClient restaurantApiClient)
    {
        _restaurantApiClient = restaurantApiClient;
    }

    [AllowAnonymous]
    [HttpGet("Login")]
    public IActionResult Login()
    {
        return Challenge(
            new AuthenticationProperties
            {
                RedirectUri = "/Manager/SelectRestaurant"
            },
            OpenIdConnectDefaults.AuthenticationScheme);
    }

    [HttpGet("SelectRestaurant")]
    public IActionResult SelectRestaurant()
    {
        var restaurants = _restaurantApiClient.GetAllRestaurants();

        return View(restaurants);
    }

    [HttpPost("SelectRestaurant")]
    [ValidateAntiForgeryToken]
    public IActionResult SelectRestaurant(int restaurantId)
    {
        var restaurants = _restaurantApiClient.GetAllRestaurants();

        var restaurant = restaurants.FirstOrDefault(
            r => r.RestaurantId == restaurantId);

        if (restaurant == null)
        {
            return NotFound();
        }

        HttpContext.Session.SetInt32("RestaurantId", restaurantId);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public IActionResult Index()
    {
        var restaurantId = HttpContext.Session.GetInt32("RestaurantId");

        if (restaurantId == null)
        {
            return RedirectToAction(nameof(SelectRestaurant));
        }

        var restaurants = _restaurantApiClient.GetAllRestaurants();

        var restaurant = restaurants.FirstOrDefault(
            r => r.RestaurantId == restaurantId.Value);

        if (restaurant == null)
        {
            HttpContext.Session.Remove("RestaurantId");
            return RedirectToAction(nameof(SelectRestaurant));
        }

        return View(restaurant);
    }

    [HttpGet("FoodItems")]
    public IActionResult FoodItems()
    {
        var restaurantId = HttpContext.Session.GetInt32("RestaurantId");

        if (restaurantId == null)
        {
            return RedirectToAction(nameof(SelectRestaurant));
        }

        var restaurant = _restaurantApiClient.GetRestaurantMenuById(restaurantId.Value);

        if (restaurant == null)
        {
            return NotFound();
        }

        return View(restaurant);
    }
}