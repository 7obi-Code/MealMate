using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IFRestaurantDao
    {
        Restaurant? GetRestaurantMenuById(int restaurantId);
        List<Restaurant> GetAllRestaurants();
    }
}
