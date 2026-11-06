
using DataAccess.Model;
using System.Collections.Generic;

namespace DataAccess.Interfaces
{
    public interface IFFoodItemIngredientDao
    {
        List<FoodItemIngredient> GetIngredientsByFoodItemId(int foodItemId);
    }
}
