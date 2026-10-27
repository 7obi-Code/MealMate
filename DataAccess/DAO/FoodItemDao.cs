
using Dapper;
using DataAccess.Interfaces;
using DataAccess.Model;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.DAO
{
    public class FoodItemDao : BaseDao, IFFoodItemDao
    {
        public FoodItemDao(string connectionString)
            : base(connectionString)
        {
        }

        public FoodItem? GetFoodItemById(int foodItemId)
        {
            using var connection = CreateConnection();

            const string sql = """
    SELECT
        "foodItemId" AS "FoodItemId",
        "name" AS "Name",
        "description" AS "Description",
        "isAvailable" AS "IsAvailable"
    FROM mealmate_v1."FoodItem"
    WHERE "foodItemId" = @FoodItemId;
    """;

            var foodItem = connection.QuerySingleOrDefault<FoodItem>(
                sql,
                new { FoodItemId = foodItemId });

            if (foodItem != null)
            {
                foodItem.Prices = GetPricesByFoodItemId(foodItemId);
            }

            return foodItem;
        }


        public List<FoodItem> GetFoodItemsByCategoryId(int categoryId)
        {
            using var connection = CreateConnection();

            const string sql = """
        SELECT
            "foodItemId" AS "FoodItemId",
            "name" AS "Name",
            "description" AS "Description",
        FROM mealmate_v1."FoodItem"
        WHERE "categoryId_fk" = @CategoryId;
        """;

            var foodItems = connection.Query<FoodItem>(
                sql,
                new { CategoryId = categoryId }
            ).ToList();

            foreach (var foodItem in foodItems)
            {
                foodItem.Prices = GetPricesByFoodItemId(foodItem.FoodItemId);
            }

            return foodItems;
        }



        public List<FoodItem> GetFoodItemsByMenuId(int menuId)
        {
            using var connection = CreateConnection();

            const string sql = """
        SELECT
            fi."foodItemId" AS "FoodItemId",
            fi."name" AS "Name",
            fi."description" AS "Description",
            fi."isAvailable" AS "IsAvailable"
        FROM mealmate_v1."FoodItem" fi
        JOIN mealmate_v1."FoodCategory" fc
            ON fc."categoryId" = fi."categoryId_fk"
        WHERE fc."menuId_fk" = @MenuId;
        """;

            var foodItems = connection.Query<FoodItem>(
                sql,
                new { MenuId = menuId }
            ).ToList();

            foreach (var foodItem in foodItems)
            {
                foodItem.Prices = GetPricesByFoodItemId(foodItem.FoodItemId);
            }

            return foodItems;
        }



        private List<Price> GetPricesByFoodItemId(int foodItemId)
        {
            using var connection = CreateConnection();

            const string sql = """
        SELECT
            "priceId" AS "PriceId",
            "amount" AS "Amount",
            "validFrom" AS "ValidFrom"
        FROM mealmate_v1."Price"
        WHERE "foodItemId_fk" = @FoodItemId;
        """;

            return connection.Query<Price>(
                sql,
                new { FoodItemId = foodItemId }
            ).ToList();
        }

    }
}
