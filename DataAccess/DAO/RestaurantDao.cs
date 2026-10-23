using DataAccess.Interfaces;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class RestaurantDao : IFRestaurantDao
    {
        private readonly IFMenuDao _menuDao;

        public RestaurantDao(IFMenuDao menuDao)
        {
            _menuDao = menuDao;
        }

        public Restaurant? GetRestaurantMenuById(int restaurantId)
        {
            var restaurant = _restaurants.FirstOrDefault(
                restaurant => restaurant.RestaurantId == restaurantId);

            if (restaurant != null)
            {
                restaurant.Menus =
                    _menuDao.GetMenusByRestaurantId(restaurantId);
            }

            return restaurant;
        }


        private readonly List<Restaurant> _restaurants = new()
        {
            new Restaurant
            {
                RestaurantId = 1,
                Name = "Pizza Palace",
                IsAcceptingOrders = true,
                Menus = new List<Menu>
                {
                    
                }
            },

            new Restaurant
            {
                RestaurantId = 2,
                Name = "Burger Boom",
                IsAcceptingOrders = true,
                Menus = new List<Menu>
                {

                }
            }
        };
    }
}