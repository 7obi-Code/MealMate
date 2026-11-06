using Dapper;
using DataAccess.Interfaces;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.DAO
{
    public class RestaurantDao : BaseDao, IFRestaurantDao
    {
        private readonly IFMenuDao _menuDao;

        public RestaurantDao(
            string connectionString,
            IFMenuDao menuDao)
            : base(connectionString)
        {
            _menuDao = menuDao;
        }
        public List<Restaurant> GetAllRestaurants()
        {
            try
            {
                using var connection = CreateConnection();

                const string sql = """
            SELECT
                "restaurantId" AS "RestaurantId",
                "name" AS "Name",
                "isAcceptingOrders" AS "IsAcceptingOrders"
            FROM mealmate_v1."Restaurant"
            ORDER BY "restaurantId";
            """;

                return connection.Query<Restaurant>(sql).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Databasefejl: {ex.Message}");
                return GetTestRestaurants();
            }
        }

        public Restaurant? GetRestaurantMenuById(int restaurantId)
        {
            try
            {
                using var connection = CreateConnection();

                const string sql = """
            SELECT
                "restaurantId" AS "RestaurantId",
                "name" AS "Name",
                "isAcceptingOrders" AS "IsAcceptingOrders"
            FROM mealmate_v1."Restaurant"
            WHERE "restaurantId" = @RestaurantId;
            """;

                var restaurant = connection.QuerySingleOrDefault<Restaurant>(
                    sql,
                    new { RestaurantId = restaurantId }
                );

                if (restaurant == null)
                    return null;

                // Hent menu, kategorier, retter og priser
                restaurant.Menus = _menuDao.GetMenusByRestaurantId(restaurantId);

                return restaurant;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Databasefejl i GetRestaurantMenuById: {ex.Message}"
                );

                // Brug kun testdata, hvis databaseforbindelsen fejler
                return GetTestRestaurants()
                    .FirstOrDefault(r => r.RestaurantId == restaurantId);
            }
        }


        private static List<Restaurant> GetTestRestaurants()
        {
            return new List<Restaurant>
    {
        // TESTRESTAURANT 1
        new Restaurant
        {
            RestaurantId = 9001,
            Name = "TEST - Burger House (Fake DB)",
            IsAcceptingOrders = true,

            Menus = new List<Menu>
            {
                new Menu
                {
                    MenuId = 9001,
                    Name = "TEST - Burgermenu",
                    IsActive = true,

                    Categories = new List<FoodCategory>
                    {
                        new FoodCategory
                        {
                            CategoryId = 9001,
                            Name = "TEST - Burgere",

                            FoodItems = new List<FoodItem>
                            {
                                new FoodItem
                                {
                                    FoodItemId = 9001,
                                    Name = "TEST - Burger (Fake DB)",
                                    Description = "TESTDATA: Burger fra hardcoded testdata.",
                                    IsAvailable = true,

                                    Prices = new List<Price>
                                    {
                                        new Price
                                        {
                                            Amount = 89,
                                            ValidFrom = DateTime.Now.AddDays(-1)
                                        }
                                    },

                                    Ingredients = new List<FoodItemIngredient>
                                    {
                                        new FoodItemIngredient
                                        {
                                            FoodItemId = 9001,
                                            IngredientId = 1,
                                            IsDefault = true,
                                            ExtraPrice = 0,
                                            Ingredient = new Ingredient
                                            {
                                                IngredientId = 1,
                                                Name = "Salat"
                                            }
                                        },
                                        new FoodItemIngredient
                                        {
                                            FoodItemId = 9001,
                                            IngredientId = 2,
                                            IsDefault = true,
                                            ExtraPrice = 0,
                                            Ingredient = new Ingredient
                                            {
                                                IngredientId = 2,
                                                Name = "Tomat"
                                            }
                                        },
                                        new FoodItemIngredient
                                        {
                                            FoodItemId = 9001,
                                            IngredientId = 3,
                                            IsDefault = true,
                                            ExtraPrice = 0,
                                            Ingredient = new Ingredient
                                            {
                                                IngredientId = 3,
                                                Name = "Ost"
                                            }
                                        },
                                        new FoodItemIngredient
                                        {
                                            FoodItemId = 9001,
                                            IngredientId = 4,
                                            IsDefault = false,
                                            ExtraPrice = 10,
                                            Ingredient = new Ingredient
                                            {
                                                IngredientId = 4,
                                                Name = "Bacon"
                                            }
                                        },
                                        new FoodItemIngredient
                                        {
                                            FoodItemId = 9001,
                                            IngredientId = 5,
                                            IsDefault = false,
                                            ExtraPrice = 5,
                                            Ingredient = new Ingredient
                                            {
                                                IngredientId = 5,
                                                Name = "Ekstra dressing"
                                            }
                                        }
                                    }
                                },

                                new FoodItem
                                {
                                    FoodItemId = 9002,
                                    Name = "TEST - Cheese Burger (Fake DB)",
                                    Description = "TESTDATA: Cheeseburger fra hardcoded testdata.",
                                    IsAvailable = true,

                                    Prices = new List<Price>
                                    {
                                        new Price
                                        {
                                            Amount = 99,
                                            ValidFrom = DateTime.Now.AddDays(-1)
                                        }
                                    },

                                    Ingredients = new List<FoodItemIngredient>
                                    {
                                        new FoodItemIngredient
                                        {
                                            FoodItemId = 9002,
                                            IngredientId = 3,
                                            IsDefault = true,
                                            ExtraPrice = 0,
                                            Ingredient = new Ingredient
                                            {
                                                IngredientId = 3,
                                                Name = "Ost"
                                            }
                                        },
                                        new FoodItemIngredient
                                        {
                                            FoodItemId = 9002,
                                            IngredientId = 6,
                                            IsDefault = true,
                                            ExtraPrice = 0,
                                            Ingredient = new Ingredient
                                            {
                                                IngredientId = 6,
                                                Name = "Løg"
                                            }
                                        },
                                        new FoodItemIngredient
                                        {
                                            FoodItemId = 9002,
                                            IngredientId = 4,
                                            IsDefault = false,
                                            ExtraPrice = 10,
                                            Ingredient = new Ingredient
                                            {
                                                IngredientId = 4,
                                                Name = "Bacon"
                                            }
                                        },
                                        new FoodItemIngredient
                                        {
                                            FoodItemId = 9002,
                                            IngredientId = 7,
                                            IsDefault = false,
                                            ExtraPrice = 8,
                                            Ingredient = new Ingredient
                                            {
                                                IngredientId = 7,
                                                Name = "Ekstra ost"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        },

        // TESTRESTAURANT 2
        new Restaurant
        {
            RestaurantId = 9002,
            Name = "TEST - Pizza Palace (Fake DB)",
            IsAcceptingOrders = true,

            Menus = new List<Menu>
            {
                new Menu
                {
                    MenuId = 9002,
                    Name = "TEST - Pizzamenu",
                    IsActive = true,

                    Categories = new List<FoodCategory>
                    {
                        new FoodCategory
                        {
                            CategoryId = 9002,
                            Name = "TEST - Pizza",

                            FoodItems = new List<FoodItem>
                            {
                                new FoodItem
                                {
                                    FoodItemId = 9003,
                                    Name = "TEST - Margherita (Fake DB)",
                                    Description = "TESTDATA: Pizza fra hardcoded testdata.",
                                    IsAvailable = true,

                                    Prices = new List<Price>
                                    {
                                        new Price
                                        {
                                            Amount = 85,
                                            ValidFrom = DateTime.Now.AddDays(-1)
                                        }
                                    },

                                    Ingredients = new List<FoodItemIngredient>
                                    {
                                        new FoodItemIngredient
                                        {
                                            FoodItemId = 9003,
                                            IngredientId = 8,
                                            IsDefault = true,
                                            ExtraPrice = 0,
                                            Ingredient = new Ingredient
                                            {
                                                IngredientId = 8,
                                                Name = "Tomatsauce"
                                            }
                                        },
                                        new FoodItemIngredient
                                        {
                                            FoodItemId = 9003,
                                            IngredientId = 9,
                                            IsDefault = true,
                                            ExtraPrice = 0,
                                            Ingredient = new Ingredient
                                            {
                                                IngredientId = 9,
                                                Name = "Mozzarella"
                                            }
                                        },
                                        new FoodItemIngredient
                                        {
                                            FoodItemId = 9003,
                                            IngredientId = 10,
                                            IsDefault = true,
                                            ExtraPrice = 0,
                                            Ingredient = new Ingredient
                                            {
                                                IngredientId = 10,
                                                Name = "Basilikum"
                                            }
                                        },
                                        new FoodItemIngredient
                                        {
                                            FoodItemId = 9003,
                                            IngredientId = 11,
                                            IsDefault = false,
                                            ExtraPrice = 12,
                                            Ingredient = new Ingredient
                                            {
                                                IngredientId = 11,
                                                Name = "Ekstra mozzarella"
                                            }
                                        },
                                        new FoodItemIngredient
                                        {
                                            FoodItemId = 9003,
                                            IngredientId = 12,
                                            IsDefault = false,
                                            ExtraPrice = 8,
                                            Ingredient = new Ingredient
                                            {
                                                IngredientId = 12,
                                                Name = "Oliven"
                                            }
                                        }
                                    }
                                },

                                new FoodItem
                                {
                                    FoodItemId = 9004,
                                    Name = "TEST - Pepperoni (Fake DB)",
                                    Description = "TESTDATA: Pepperonipizza fra hardcoded testdata.",
                                    IsAvailable = true,

                                    Prices = new List<Price>
                                    {
                                        new Price
                                        {
                                            Amount = 95,
                                            ValidFrom = DateTime.Now.AddDays(-1)
                                        }
                                    },

                                    Ingredients = new List<FoodItemIngredient>
                                    {
                                        new FoodItemIngredient
                                        {
                                            FoodItemId = 9004,
                                            IngredientId = 8,
                                            IsDefault = true,
                                            ExtraPrice = 0,
                                            Ingredient = new Ingredient
                                            {
                                                IngredientId = 8,
                                                Name = "Tomatsauce"
                                            }
                                        },
                                        new FoodItemIngredient
                                        {
                                            FoodItemId = 9004,
                                            IngredientId = 13,
                                            IsDefault = true,
                                            ExtraPrice = 0,
                                            Ingredient = new Ingredient
                                            {
                                                IngredientId = 13,
                                                Name = "Pepperoni"
                                            }
                                        },
                                        new FoodItemIngredient
                                        {
                                            FoodItemId = 9004,
                                            IngredientId = 9,
                                            IsDefault = true,
                                            ExtraPrice = 0,
                                            Ingredient = new Ingredient
                                            {
                                                IngredientId = 9,
                                                Name = "Mozzarella"
                                            }
                                        },
                                        new FoodItemIngredient
                                        {
                                            FoodItemId = 9004,
                                            IngredientId = 14,
                                            IsDefault = false,
                                            ExtraPrice = 10,
                                            Ingredient = new Ingredient
                                            {
                                                IngredientId = 14,
                                                Name = "Ekstra pepperoni"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    };
  }
}
}