using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Model
{
    public class KitchenStation
    {
        public string Name { get; set; } = string.Empty;
        public int CapacityPerInterval { get; set; }
        public int IntervalMinutes { get; set; }
        public List<StationCapacitySlot> CapacitySlots { get; set; } = new();
    }
}
