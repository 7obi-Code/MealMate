using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Interfaces
{
    public interface IFMenuDao
    {
        List<Menu> GetMenusByRestaurantId(int restaurantId);
    }
}
