namespace Website.Models;

public class CartItem
{
    public int FoodItemId { get; set; }
    public int RestaurantId { get; set; }
    public string Name { get; set; } = "";
    public decimal BasePrice { get; set; }
    public int Quantity { get; set; }
    public List<CartItemIngredient> SelectedIngredients { get; set; } = new();
    public decimal UnitPrice =>
        BasePrice + SelectedIngredients.Sum(i => i.ExtraPrice);
    public decimal SubTotal => UnitPrice * Quantity;
}