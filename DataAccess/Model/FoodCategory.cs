using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess.Model
{
    public class FoodCategory
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<FoodItem> FoodItems { get; set; } = new();
    }
}