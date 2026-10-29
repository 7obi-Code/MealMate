
using APIClient;
using Microsoft.AspNetCore.Mvc;
using Website.Extensions;
using Website.Models;

namespace Website.Controllers;

public class CartController : Controller
{
    private const string CartSessionKey = "MealMateCart";
    private readonly RestaurantApiClient _restaurantApiClient;

    public CartController(RestaurantApiClient restaurantApiClient)
    {
        _restaurantApiClient = restaurantApiClient;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var cart = HttpContext.Session
            .GetObject<List<CartItem>>(CartSessionKey) ?? new();

        return View(cart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(int foodItemId, int restaurantId, int quantity = 1)
    {
        if (quantity < 1)
            quantity = 1;

        var restaurant =
            _restaurantApiClient.GetRestaurantMenuById(restaurantId);

        if (restaurant == null || !restaurant.IsAcceptingOrders)
            return NotFound();

        var foodItem = restaurant.Menus
            .Where(menu => menu.IsActive)
            .SelectMany(menu => menu.Categories)
            .SelectMany(category => category.FoodItems)
            .FirstOrDefault(item =>
                item.FoodItemId == foodItemId && item.IsAvailable);

        if (foodItem == null)
            return NotFound();

        var currentPrice = foodItem.Prices
            .OrderByDescending(price => price.ValidFrom)
            .FirstOrDefault();

        if (currentPrice == null)
            return BadRequest("Retten har ingen tilgængelig pris.");

        var cart = HttpContext.Session
            .GetObject<List<CartItem>>(CartSessionKey) ?? new();

        // Tjek om kurven allerede indeholder varer fra en anden restaurant
        if (cart.Any(item => item.RestaurantId != restaurantId))
        {
            TempData["CartWarning"] =
                "Din kurv indeholder allerede varer fra en anden restaurant. " +
                "Fjern varerne fra kurven, før du vælger en anden restaurant.";

            return RedirectToAction(
                "Index",
                "Restaurant",
                new { id = restaurantId, cartOpen = true });
        }

        var existingItem = cart.FirstOrDefault(item =>
            item.FoodItemId == foodItemId &&
            item.RestaurantId == restaurantId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            cart.Add(new CartItem
            {
                FoodItemId = foodItem.FoodItemId,
                RestaurantId = restaurantId,
                Name = foodItem.Name,
                UnitPrice = currentPrice.Amount,
                Quantity = quantity
            });
        }

        HttpContext.Session.SetObject(CartSessionKey, cart);

        return RedirectToAction("Index", "Restaurant",
            new { id = restaurantId, cartOpen = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(int foodItemId, int restaurantId)
    {
        var cart = HttpContext.Session
            .GetObject<List<CartItem>>(CartSessionKey) ?? new();

        cart.RemoveAll(item =>
            item.FoodItemId == foodItemId &&
            item.RestaurantId == restaurantId);

        HttpContext.Session.SetObject(CartSessionKey, cart);

        return RedirectToAction(nameof(Index));
    }
}
