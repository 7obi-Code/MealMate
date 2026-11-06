using System.Collections.Generic;

namespace DataAccess.Model
{
    public class FoodItem
    {
        public int FoodItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public List<Price> Prices { get; set; } = new();
        public List<FoodItemIngredient> Ingredients { get; set; } = new();
    }
}