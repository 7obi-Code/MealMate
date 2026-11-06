namespace DataAccess.Model
{
    public class FoodItemIngredient
    {
        public int FoodItemId { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; } = new();
        public bool IsDefault { get; set; }
        public decimal ExtraPrice { get; set; }
    }
}