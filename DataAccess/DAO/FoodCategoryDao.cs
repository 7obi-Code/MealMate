
using System.Collections.Generic;
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
        connection.Open();

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
                    WHERE "foodItemId" = ANY(@FoodItemIds)
                    AND "categoryId_fk" IN (
                        SELECT "categoryId"
                        FROM mealmate_v1."FoodCategory"
                        WHERE "menuId_fk" = @MenuId
                    );
                    """;

                connection.Execute(
                    updateSql,
                    new
                    {
                        CategoryId = categoryId,
                        FoodItemIds = foodItemIds.ToArray(),
                        MenuId = menuId
                    },
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

    public bool UpdateCategory(
        int categoryId,
        int menuId,
        string name,
        List<int> foodItemIds)
    {
        using var connection = CreateConnection();
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            const string checkCategorySql = """
                SELECT EXISTS (
                    SELECT 1
                    FROM mealmate_v1."FoodCategory"
                    WHERE "categoryId" = @CategoryId
                    AND "menuId_fk" = @MenuId
                );
                """;

            bool categoryExists = connection.QuerySingle<bool>(
                checkCategorySql,
                new { CategoryId = categoryId, MenuId = menuId },
                transaction);

            if (!categoryExists)
            {
                transaction.Rollback();
                return false;
            }

            // Validate that selected food items belong to this menu.
            if (foodItemIds.Count > 0)
            {
                const string checkItemsSql = """
                    SELECT COUNT(*)
                    FROM mealmate_v1."FoodItem" fi
                    JOIN mealmate_v1."FoodCategory" fc
                        ON fi."categoryId_fk" = fc."categoryId"
                    WHERE fi."foodItemId" = ANY(@FoodItemIds)
                    AND fc."menuId_fk" = @MenuId;
                    """;

                int validItems = connection.QuerySingle<int>(
                    checkItemsSql,
                    new
                    {
                        FoodItemIds = foodItemIds.ToArray(),
                        MenuId = menuId
                    },
                    transaction);

                if (validItems != foodItemIds.Count)
                {
                    transaction.Rollback();
                    return false;
                }
            }

            // Update category name.
            const string updateCategorySql = """
                UPDATE mealmate_v1."FoodCategory"
                SET "name" = @Name
                WHERE "categoryId" = @CategoryId;
                """;

            connection.Execute(
                updateCategorySql,
                new { CategoryId = categoryId, Name = name },
                transaction);

            // Remove existing food items from this category.
            const string removeItemsSql = """
                UPDATE mealmate_v1."FoodItem"
                SET "categoryId_fk" = NULL
                WHERE "categoryId_fk" = @CategoryId;
                """;

            connection.Execute(
                removeItemsSql,
                new { CategoryId = categoryId },
                transaction);

            // Assign selected food items to the category.
            if (foodItemIds.Count > 0)
            {
                const string assignItemsSql = """
                    UPDATE mealmate_v1."FoodItem"
                    SET "categoryId_fk" = @CategoryId
                    WHERE "foodItemId" = ANY(@FoodItemIds);
                    """;

                connection.Execute(
                    assignItemsSql,
                    new
                    {
                        CategoryId = categoryId,
                        FoodItemIds = foodItemIds.ToArray()
                    },
                    transaction);
            }

            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public bool DeleteCategory(int categoryId)
    {
        using var connection = CreateConnection();
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            // Remove food items from the category.
            const string updateSql = """
                UPDATE mealmate_v1."FoodItem"
                SET "categoryId_fk" = NULL
                WHERE "categoryId_fk" = @CategoryId;
                """;

            connection.Execute(
                updateSql,
                new { CategoryId = categoryId },
                transaction);

            // Delete the category.
            const string deleteSql = """
                DELETE FROM mealmate_v1."FoodCategory"
                WHERE "categoryId" = @CategoryId;
                """;

            int rowsAffected = connection.Execute(
                deleteSql,
                new { CategoryId = categoryId },
                transaction);

            transaction.Commit();

            return rowsAffected > 0;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
