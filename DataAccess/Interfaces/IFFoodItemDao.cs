using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Interfaces
{
    public interface IFFoodItemDao
    {
        List<FoodItem> GetFoodItemsByCategoryId(int categoryId);
        List<FoodItem> GetFoodItemsByMenuId(int menuId);
        FoodItem? GetFoodItemById(int foodItemId);
    }
}
