
using Dapper;
using DataAccess.Interfaces;
using DataAccess.Model;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.DAO
{
    public class MenuDao : BaseDao, IFMenuDao
    {
        private readonly IFFoodItemDao _foodItemDao;

        public MenuDao(
    string connectionString,
    IFFoodItemDao foodItemDao)
    : base(connectionString)
        {
            _foodItemDao = foodItemDao;
        }

        public List<Menu> GetMenusByRestaurantId(int restaurantId)
        {
            using var connection = CreateConnection();

            const string menuSql = """
                SELECT
                    "menuId" AS "MenuId",
                    "restaurantId_fk" AS "RestaurantId",
                    "name" AS "Name",
                    "isActive" AS "IsActive"
                FROM mealmate_v1."Menu"
                WHERE "restaurantId_fk" = @RestaurantId;
                """;

            var menus = connection.Query<Menu>(
                menuSql,
                new { RestaurantId = restaurantId }
            ).ToList();

            foreach (var menu in menus)
            {
                const string categorySql = """
                    SELECT
                        "categoryId" AS "CategoryId",
                        "name" AS "Name"
                    FROM mealmate_v1."FoodCategory"
                    WHERE "menuId_fk" = @MenuId;
                    """;

                menu.Categories = connection.Query<FoodCategory>(
                    categorySql,
                    new { MenuId = menu.MenuId }
                ).ToList();

                foreach (var category in menu.Categories)
                {
                    category.FoodItems =
                        _foodItemDao.GetFoodItemsByCategoryId(
                            category.CategoryId);
                }
            }

            return menus;
        }
    }
}
