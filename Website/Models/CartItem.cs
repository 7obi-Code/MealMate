namespace Website.Models;
public class CartItem
{
    public int FoodItemId { get; set; }
    public int RestaurantId { get; set; }
    public string Name { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }

    public decimal SubTotal => UnitPrice * Quantity;
}
