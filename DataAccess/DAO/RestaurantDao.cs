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