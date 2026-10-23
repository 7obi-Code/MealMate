using DataAccess.Interfaces;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.DAO
{
    public class MenuDao : IFMenuDao
    {

        public List<Menu> GetMenusByRestaurantId(int restaurantId) =>
        _menus
        .Where(menu => menu.RestaurantId == restaurantId)
        .ToList();

        private readonly List<Menu> _menus = new()
        {
            new Menu
            {
                        MenuId = 1,
                        RestaurantId = 1,
                        Name = "Main Menu",
                        IsActive = true,
                        Categories = new List<FoodCategory>
                        {
                            new FoodCategory
                            {
                                CategoryId = 1,
                                Name = "Pizza",
                                FoodItems = new List<FoodItem>
                                {
                                    new FoodItem
                                    {
                                        FoodItemId = 1,
                                        Name = "Margherita",
                                        Description = "Tomato, mozzarella and fresh basil.",
                                        PreparationMinutes = 12,
                                        IsAvailable = true,
                                        Prices = new List<Price>
                                        {
                                            new Price
                                            {
                                                PriceId = 1,
                                                Amount = 79,
                                                ValidFrom = DateTime.UtcNow
                                            }
                                        }
                                    },
                                    new FoodItem
                                    {
                                        FoodItemId = 2,
                                        Name = "Pepperoni",
                                        Description = "Tomato, mozzarella and pepperoni.",
                                        PreparationMinutes = 15,
                                        IsAvailable = true,
                                        Prices = new List<Price>
                                        {
                                            new Price
                                            {
                                                PriceId = 2,
                                                Amount = 89,
                                                ValidFrom = DateTime.UtcNow
                                            }
                                        }
                                    }
                                }
                            },
                            new FoodCategory
                            {
                                CategoryId = 2,
                                Name = "Drinks",
                                FoodItems = new List<FoodItem>
                                {
                                    new FoodItem
                                    {
                                        FoodItemId = 3,
                                        Name = "Cola",
                                        Description = "Cold soft drink.",
                                        PreparationMinutes = 1,
                                        IsAvailable = true,
                                        Prices = new List<Price>
                                        {
                                            new Price
                                            {
                                                PriceId = 3,
                                                Amount = 25,
                                                ValidFrom = DateTime.UtcNow
                                            }
                                        }
                                    }
                                }
                            }
                        }
            },

            new Menu
            {
                        MenuId = 2,
                        RestaurantId = 2,
                        Name = "Main Menu",
                        IsActive = true,
                        Categories = new List<FoodCategory>
                        {
                            new FoodCategory
                            {
                                CategoryId = 3,
                                Name = "Burgers",
                                FoodItems = new List<FoodItem>
                                {
                                    new FoodItem
                                    {
                                        FoodItemId = 4,
                                        Name = "Classic Burger",
                                        Description = "Beef patty, lettuce, tomato and sauce.",
                                        PreparationMinutes = 10,
                                        IsAvailable = true,
                                        Prices = new List<Price>
                                        {
                                            new Price
                                            {
                                                PriceId = 4,
                                                Amount = 9,
                                                ValidFrom = DateTime.UtcNow
                                            }
                                        }
                                    },
                                    new FoodItem
                                    {
                                        FoodItemId = 5,
                                        Name = "Cheeseburger",
                                        Description = "Beef patty with melted cheese.",
                                        PreparationMinutes = 12,
                                        IsAvailable = true,
                                        Prices = new List<Price>
                                        {
                                            new Price
                                            {
                                                PriceId = 5,
                                                Amount = 109,
                                                ValidFrom = DateTime.UtcNow
                                            }
                                        }
                                    }
                                }
                            }
                        }
            }
        };
    };
}

