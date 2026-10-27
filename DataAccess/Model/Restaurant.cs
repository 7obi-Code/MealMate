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
        public string Name { get; set; } = string.Empty;
        public bool IsAcceptingOrders { get; set; }

        public Menu? Menu { get; set; }
        public List<OrderingPause> OrderingPauses { get; set; } = new();
    }
}

