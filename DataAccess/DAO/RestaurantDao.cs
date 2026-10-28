
using Dapper;
using DataAccess.Interfaces;
using DataAccess.Model;
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

        public Restaurant? GetRestaurantMenuById(int restaurantId)
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

            restaurant.Menus =
                _menuDao.GetMenusByRestaurantId(restaurantId);

            return restaurant;
        }
    }
}