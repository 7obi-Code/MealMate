using APIClient;
using DataAccess.Model;
using DataAccess.DTOs;
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
    private readonly FoodItemApiClient _foodItemApiClient;
    private readonly FoodCategoryApiClient _foodCategoryApiClient;

    public ManagerController (RestaurantApiClient restaurantApiClient, FoodItemApiClient foodItemApiClient, FoodCategoryApiClient foodCategoryApiClient)
    {
        _restaurantApiClient = restaurantApiClient;
        _foodItemApiClient = foodItemApiClient;
        _foodCategoryApiClient = foodCategoryApiClient;
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

    //Manager Funktion
    [HttpPost("FoodItems/Create")]
    [ValidateAntiForgeryToken]
    public IActionResult CreateFoodItem(CreateFoodItemRequest request)
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

        bool categoryExists = restaurant.Menus
            .SelectMany(m => m.Categories)
            .Any(c => c.CategoryId == request.CategoryId);

        if (!categoryExists)
        {
            return BadRequest("Invalid category.");
        }

        if (string.IsNullOrWhiteSpace(request.Name) || request.Amount < 0)
        {
            return BadRequest("Invalid food item information.");
        }

        var result = _foodItemApiClient.CreateFoodItem(request);

        if (result == null)
        {
            TempData["Error"] = "Could not create food item.";
        }
        else
        {
            TempData["Success"] = "Food item created successfully.";
        }

        return RedirectToAction(nameof(FoodItems));
    }

    //Manager Funktion
    [HttpPost("FoodItems/Edit/{foodItemId}")]
    [ValidateAntiForgeryToken]
    public IActionResult EditFoodItem(int foodItemId, UpdateFoodItemRequest request)
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

        bool foodItemExists = restaurant.Menus
            .SelectMany(m => m.Categories)
            .SelectMany(c => c.FoodItems)
            .Any(f => f.FoodItemId == foodItemId);

        if (!foodItemExists)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(request.Name) || request.Amount < 0)
        {
            return BadRequest("Invalid food item information.");
        }

        bool updated = _foodItemApiClient.UpdateFoodItem(foodItemId, request);

        TempData[updated ? "Success" : "Error"] =
            updated ? "Food item updated successfully." : "Could not update food item.";

        return RedirectToAction(nameof(FoodItems));
    }

    //Manager Funktion
    [HttpPost("FoodItems/Delete/{foodItemId}")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteFoodItem(int foodItemId)
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

        bool foodItemExists = restaurant.Menus
            .SelectMany(m => m.Categories)
            .SelectMany(c => c.FoodItems)
            .Any(f => f.FoodItemId == foodItemId);

        if (!foodItemExists)
        {
            return NotFound();
        }

        bool deleted = _foodItemApiClient.DeleteFoodItem(foodItemId);

        TempData[deleted ? "Success" : "Error"] =
            deleted ? "Food item deleted successfully." : "Could not delete food item.";

        return RedirectToAction(nameof(FoodItems));
    }

    //Manager Funktion
    [HttpGet("Categories")]
    public IActionResult Categories()
    {
        var restaurantId = HttpContext.Session.GetInt32("RestaurantId");

        if (restaurantId == null)
            return RedirectToAction(nameof(SelectRestaurant));

        var restaurant = _restaurantApiClient.GetRestaurantMenuById(restaurantId.Value);

        if (restaurant == null)
            return NotFound();

        return View(restaurant);
    }

    //Manager Funktion
    [HttpPost("Categories/Create")]
    [ValidateAntiForgeryToken]
    public IActionResult CreateCategory(CreateFoodCategoryRequest request)
    {
        var restaurantId = HttpContext.Session.GetInt32("RestaurantId");

        if (restaurantId == null)
            return RedirectToAction(nameof(SelectRestaurant));

        var restaurant = _restaurantApiClient.GetRestaurantMenuById(restaurantId.Value);

        if (restaurant == null)
            return NotFound();

        var menu = restaurant.Menus.FirstOrDefault(m => m.MenuId == request.MenuId);

        if (menu == null || string.IsNullOrWhiteSpace(request.Name))
            return BadRequest();

        var availableIds = menu.Categories
            .SelectMany(c => c.FoodItems)
            .Select(f => f.FoodItemId)
            .ToHashSet();

        if (request.FoodItemIds.Any(id => !availableIds.Contains(id)))
            return BadRequest("Invalid food item.");

        bool created = _foodCategoryApiClient.CreateCategory(request);

        TempData[created ? "Success" : "Error"] =
            created ? "Category created successfully." : "Could not create category.";

        return RedirectToAction(nameof(Categories));
    }

}