
using APIClient;
using BusinessLogic.Interfaces;
using DataAccess.Model;
using Microsoft.AspNetCore.Mvc;
using Website.Extensions;
using Website.Models;

namespace Website.Controllers;

public class CartController : Controller
{
    private const string CartSessionKey = "MealMateCart";
    private const string WaitSessionKey = "MealMateEstimatedWaitMinutes";

    private readonly RestaurantApiClient _restaurantApiClient;
    private readonly IFPickupTimeService _pickupTimeService;

    public CartController(
        RestaurantApiClient restaurantApiClient,
        IFPickupTimeService pickupTimeService)
    {
        _restaurantApiClient = restaurantApiClient;
        _pickupTimeService = pickupTimeService;
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

        var currentPrice = foodItem.Prices
            .OrderByDescending(price => price.ValidFrom)
            .FirstOrDefault();

        if (currentPrice == null)
            return BadRequest("Retten har ingen tilgængelig pris.");

        var availableIngredients = foodItem.Ingredients
            .Where(i => i.Ingredient != null)
            .ToList();

        var selectedIds = customizationSubmitted
            ? selectedIngredientIds ?? new List<int>()
            : availableIngredients
                .Where(i => i.IsDefault)
                .Select(i => i.IngredientId)
                .ToList();

        if (selectedIds.Except(
                availableIngredients.Select(i => i.IngredientId))
            .Any())
        {
            return BadRequest("Ugyldigt ingrediensvalg.");
        }

        var selectedIngredients = availableIngredients
            .Where(i => selectedIds.Contains(i.IngredientId))
            .Select(i => new CartItemIngredient
            {
                IngredientId = i.IngredientId,
                Name = i.Ingredient!.Name,
                ExtraPrice = i.ExtraPrice
            })
            .ToList();

        var removedIngredients = availableIngredients
            .Where(i =>
                i.IsDefault &&
                !selectedIds.Contains(i.IngredientId))
            .Select(i => i.Ingredient!.Name)
            .ToList();

        var cart = HttpContext.Session
            .GetObject<List<CartItem>>(CartSessionKey) ?? new();

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

        // Gem kurven.
        HttpContext.Session.SetObject(CartSessionKey, cart);

        // Beregn og gem forventet ventetid.
        int waitMinutes = CalculateEstimatedWaitMinutes(
            cart,
            restaurantId);

        HttpContext.Session.SetInt32(
            WaitSessionKey,
            waitMinutes);

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

        // Opdater ventetiden efter fjernelse.
        int waitMinutes = CalculateEstimatedWaitMinutes(
            cart,
            restaurantId);

        HttpContext.Session.SetInt32(
            WaitSessionKey,
            waitMinutes);

        return RedirectToAction(
            "Index",
            "Restaurant",
            new { id = restaurantId, cartOpen = true });
    }

    // Beregner ventetid med en midlertidig teststation.
    private int CalculateEstimatedWaitMinutes(
        List<CartItem> cart,
        int restaurantId)
    {
        if (!cart.Any())
            return 0;

        var restaurant =
            _restaurantApiClient.GetRestaurantMenuById(restaurantId);

        if (restaurant == null)
            return 0;

        var foodItems = restaurant.Menus
            .Where(menu => menu.IsActive)
            .SelectMany(menu => menu.Categories)
            .SelectMany(category => category.FoodItems)
            .ToList();

        var order = new Order
        {
            CreatedAt = DateTime.Now,
            OrderStatus = "Cart",
            OrderType = "Pickup"
        };

        foreach (var cartItem in cart)
        {
            var foodItem = foodItems.FirstOrDefault(
                item => item.FoodItemId == cartItem.FoodItemId);

            if (foodItem == null)
                continue;

            order.OrderLines.Add(new OrderLine
            {
                Order = order,
                FoodItemId = foodItem.FoodItemId,
                FoodItem = foodItem,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.UnitPrice,
                SubTotal = cartItem.SubTotal
            });
        }

        if (!order.OrderLines.Any())
            return 0;

        // Midlertidig teststation.
        var station = new KitchenStation
        {
            Name = "Testkøkken",
            CapacityPerInterval = 3,
            IntervalMinutes = 10
        };

        // Opret 12 ledige tidsintervaller.
        station.CapacitySlots = Enumerable.Range(0, 12)
            .Select(i => new StationCapacitySlot
            {
                SlotId = i + 1,
                StartTime = order.CreatedAt.AddMinutes(
                    i * station.IntervalMinutes),
                ReservedQuantity = 0
            })
            .ToList();

        var readyTime = _pickupTimeService
            .CalculateEstimatedReadyTime(order, station);

        return Math.Max(
            0,
            (int)Math.Ceiling(
                (readyTime - DateTime.Now).TotalMinutes));
    }
}
