using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using DataAccess.Interfaces;

namespace DataAccess.DAO;

public class FoodCategoryDao : BaseDao, IFFoodCategoryDao
{
    public FoodCategoryDao(string connectionString) : base(connectionString)
    {
    }

    public int CreateCategory(int menuId, string name, List<int> foodItemIds)
    {
        using var connection = CreateConnection();
        using var transaction = connection.BeginTransaction();

        try
        {
            const string sql = """
                INSERT INTO mealmate_v1."FoodCategory"
                    ("menuId_fk", "name")
                VALUES
                    (@MenuId, @Name)
                RETURNING "categoryId";
                """;

            int categoryId = connection.QuerySingle<int>(
                sql,
                new { MenuId = menuId, Name = name },
                transaction);

            if (foodItemIds.Count > 0)
            {
                const string updateSql = """
                    UPDATE mealmate_v1."FoodItem"
                    SET "categoryId_fk" = @CategoryId
                    WHERE "foodItemId" IN @FoodItemIds
                    AND "categoryId_fk" IN (
                        SELECT "categoryId"
                        FROM mealmate_v1."FoodCategory"
                        WHERE "menuId_fk" = @MenuId
                    );
                    """;

                connection.Execute(
                    updateSql,
                    new { CategoryId = categoryId, FoodItemIds = foodItemIds, MenuId = menuId },
                    transaction);
            }

            transaction.Commit();
            return categoryId;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}

