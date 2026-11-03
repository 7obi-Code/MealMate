
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using APIClient;
using DataAccess.Model;

namespace Website.Controllers;

public class HomeController : Controller
{
    private readonly RestaurantApiClient _restaurantApiClient;

    public HomeController(RestaurantApiClient restaurantApiClient)
    {
        _restaurantApiClient = restaurantApiClient;
    }

    public IActionResult Index()
    {
        List<Restaurant> restaurants =
            _restaurantApiClient.GetAllRestaurants();

        return View(restaurants);
    }

    // Error Handling
    [Route("/Error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public IActionResult Error() => View(new ErrorViewModel
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
    });
}
