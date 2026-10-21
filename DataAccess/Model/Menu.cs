using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public List<FoodCategory> Categories { get; set; } = new();
    }
}
