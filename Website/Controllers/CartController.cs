
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
    public IActionResult Add(
        int foodItemId,
        int restaurantId,
        int quantity = 1,
        List<int>? selectedIngredientIds = null,
        bool customizationSubmitted = false)
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

        // Find rettens aktuelle pris.
        var currentPrice = foodItem.Prices
            .OrderByDescending(price => price.ValidFrom)
            .FirstOrDefault();

        if (currentPrice == null)
            return BadRequest("Retten har ingen tilgængelig pris.");

        // Find ingredienserne, der hører til retten.
        var availableIngredients = foodItem.Ingredients
            .Where(i => i.Ingredient != null)
            .ToList();

        // Find kundens valgte ingredienser.
        var selectedIds = customizationSubmitted
            ? selectedIngredientIds ?? new List<int>()
            : availableIngredients
                .Where(i => i.IsDefault)
                .Select(i => i.IngredientId)
                .ToList();

        // Valider ingrediensvalgene.
        if (selectedIds.Except(
                availableIngredients.Select(i => i.IngredientId))
            .Any())
        {
            return BadRequest("Ugyldigt ingrediensvalg.");
        }

        // Opret listen over valgte ingredienser.
        var selectedIngredients = availableIngredients
            .Where(i => selectedIds.Contains(i.IngredientId))
            .Select(i => new CartItemIngredient
            {
                IngredientId = i.IngredientId,
                Name = i.Ingredient!.Name,
                ExtraPrice = i.ExtraPrice
            })
            .ToList();

        // Find standardingredienser, som kunden har fravalgt.
        var removedIngredients = availableIngredients
            .Where(i =>
                i.IsDefault &&
                !selectedIds.Contains(i.IngredientId))
            .Select(i => i.Ingredient!.Name)
            .ToList();

        var cart = HttpContext.Session
            .GetObject<List<CartItem>>(CartSessionKey) ?? new();

        // Tjek om kurven indeholder varer fra en anden restaurant.
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

        // Find kun en eksisterende linje, hvis ingrediensvalgene er ens.
        var existingItem = cart.FirstOrDefault(item =>
            item.FoodItemId == foodItemId &&
            item.RestaurantId == restaurantId &&
            item.SelectedIngredients
                .Select(i => i.IngredientId)
                .OrderBy(id => id)
                .SequenceEqual(
                    selectedIngredients
                        .Select(i => i.IngredientId)
                        .OrderBy(id => id)));

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
            existingItem.RemovedIngredients = removedIngredients;
        }
        else
        {
            cart.Add(new CartItem
            {
                FoodItemId = foodItem.FoodItemId,
                RestaurantId = restaurantId,
                Name = foodItem.Name,
                BasePrice = currentPrice.Amount,
                Quantity = quantity,
                SelectedIngredients = selectedIngredients,
                RemovedIngredients = removedIngredients
            });
        }

        // Gem kurven i session.
        HttpContext.Session.SetObject(CartSessionKey, cart);

        return RedirectToAction(
            "Index",
            "Restaurant",
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

        return RedirectToAction(
            "Index",
            "Restaurant",
    }
}
