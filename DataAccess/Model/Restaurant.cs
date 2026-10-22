using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; } = "";
        public bool IsAcceptingOrders { get; set; }

        public List<Menu> Menus { get; set; } = new();
    }
}
