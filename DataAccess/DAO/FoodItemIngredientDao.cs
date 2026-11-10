using Dapper;
using DataAccess.Interfaces;
using DataAccess.Model;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.DAO
{
    public class FoodItemIngredientDao : BaseDao, IFFoodItemIngredientDao
    {
        public FoodItemIngredientDao(string connectionString)
            : base(connectionString)
        {
        }
        public List<FoodItemIngredient> GetIngredientsByFoodItemId(int foodItemId)
        {
            using var connection = CreateConnection();

            const string sql = """
                SELECT
                    fii."foodItemId_fk" AS "FoodItemId",
                    fii."isDefault" AS "IsDefault",
                    fii."extraPrice" AS "ExtraPrice",
                    i."ingredientId" AS "IngredientId",
                    i."name" AS "Name"
                FROM mealmate_v1."FoodItemIngredient" fii
                INNER JOIN mealmate_v1."Ingredient" i
                    ON i."ingredientId" = fii."ingredientId_fk"
                WHERE fii."foodItemId_fk" = @FoodItemId
                ORDER BY i."name";
                """;

            return connection.Query<FoodItemIngredient, Ingredient, FoodItemIngredient>(
                sql,
                (foodItemIngredient, ingredient) =>
                {
                    foodItemIngredient.IngredientId = ingredient.IngredientId;
                    foodItemIngredient.Ingredient = ingredient;

                    return foodItemIngredient;
                },
                new { FoodItemId = foodItemId },
                splitOn: "IngredientId"
            ).ToList();
        }
    }
}