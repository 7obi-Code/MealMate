using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class FoodItem
    {
        public int FoodItemId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int PreparationMinutes { get; set; }
        public bool IsAvailable { get; set; }

        public List<Price> Prices { get; set; } = new();
    }
}
