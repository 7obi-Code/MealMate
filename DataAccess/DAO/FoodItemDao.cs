
using Dapper;
using DataAccess.Interfaces;
using DataAccess.Model;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.DAO
{
    public class FoodItemDao : BaseDao, IFFoodItemDao
    {
        private readonly IFFoodItemIngredientDao _foodItemIngredientDao;

        public FoodItemDao(
            string connectionString,
            IFFoodItemIngredientDao foodItemIngredientDao)
            : base(connectionString)
        {
            _foodItemIngredientDao = foodItemIngredientDao;
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
                LoadFoodItemDetails(foodItem);
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
                    "isAvailable" AS "IsAvailable"
                FROM mealmate_v1."FoodItem"
                WHERE "categoryId_fk" = @CategoryId;
                """;

            var foodItems = connection.Query<FoodItem>(
                sql,
                new { CategoryId = categoryId }
            ).ToList();

            foreach (var foodItem in foodItems)
            {
                LoadFoodItemDetails(foodItem);
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
                LoadFoodItemDetails(foodItem);
            }

            return foodItems;
        }

        private void LoadFoodItemDetails(FoodItem foodItem)
        {
            foodItem.Prices = GetPricesByFoodItemId(foodItem.FoodItemId);

            foodItem.Ingredients =
                _foodItemIngredientDao.GetIngredientsByFoodItemId(
                    foodItem.FoodItemId);
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

        //Opret fooditems til manager funktion
        public int CreateFoodItem(FoodItem foodItem, int categoryId, decimal amount)
        {
            using var connection = CreateConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
                const string foodItemSql = """
            INSERT INTO mealmate_v1."FoodItem"
            (
                "categoryId_fk",
                "name",
                "description",
                "isAvailable"
            )
            VALUES
            (
                @CategoryId,
                @Name,
                @Description,
                @IsAvailable
            )
            RETURNING "foodItemId";
            """;

                int foodItemId = connection.QuerySingle<int>(
                    foodItemSql,
                    new
                    {
                        CategoryId = categoryId,
                        foodItem.Name,
                        foodItem.Description,
                        foodItem.IsAvailable
                    },
                    transaction
                );

                const string priceSql = """
            INSERT INTO mealmate_v1."Price"
            (
                "foodItemId_fk",
                "amount"
            )
            VALUES
            (
                @FoodItemId,
                @Amount
            );
            """;

                connection.Execute(
                    priceSql,
                    new
                    {
                        FoodItemId = foodItemId,
                        Amount = amount
                    },
                    transaction
                );

                transaction.Commit();

                return foodItemId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        //Alter fooditems til manager funktion
        public bool UpdateFoodItem(FoodItem foodItem, decimal amount)
        {
            using var connection = CreateConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
                const string foodItemSql = """
            UPDATE mealmate_v1."FoodItem"
            SET
                "name" = @Name,
                "description" = @Description,
                "isAvailable" = @IsAvailable
            WHERE "foodItemId" = @FoodItemId;
            """;

                int affectedRows = connection.Execute(
                    foodItemSql,
                    foodItem,
                    transaction
                );

                if (affectedRows == 0)
                {
                    transaction.Rollback();
                    return false;
                }

                const string priceSql = """
            UPDATE mealmate_v1."Price"
            SET "amount" = @Amount
            WHERE "foodItemId_fk" = @FoodItemId;
            """;

                connection.Execute(
                    priceSql,
                    new
                    {
                        foodItem.FoodItemId,
                        Amount = amount
                    },
                    transaction
                );

                transaction.Commit();

                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        //Fjern fooditems til manager funktion
        public bool DeleteFoodItem(int foodItemId)
        {
            using var connection = CreateConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
                const string priceSql = """
            DELETE FROM mealmate_v1."Price"
            WHERE "foodItemId_fk" = @FoodItemId;
            """;

                connection.Execute(
                    priceSql,
                    new { FoodItemId = foodItemId },
                    transaction
                );

                const string foodItemSql = """
            DELETE FROM mealmate_v1."FoodItem"
            WHERE "foodItemId" = @FoodItemId;
            """;

                int affectedRows = connection.Execute(
                    foodItemSql,
                    new { FoodItemId = foodItemId },
                    transaction
                );

                transaction.Commit();

                return affectedRows > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }


    }
}
