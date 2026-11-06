namespace Website.Models;

public class CartItemIngredient
{
    public int IngredientId { get; set; }
    public string Name { get; set; } = "";
    public decimal ExtraPrice { get; set; }
}