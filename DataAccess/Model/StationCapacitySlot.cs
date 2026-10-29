using System;

namespace DataAccess.Model
{
    // Represents a time interval for a kitchen station and tracks
    // how much of the station's production capacity is reserved.
    public class StationCapacitySlot
    {
        // Unique identifier for the capacity slot.
        public int SlotId { get; set; }

        // The start time of the interval (e.g. 12:00).
        public DateTime StartTime { get; set; }

        // Number of food items already reserved in this interval.
        public int ReservedQuantity { get; set; }

        // Calculates the remaining capacity for this time interval
        // based on the associated kitchen station's maximum capacity.
        public int GetAvailableCapacity(KitchenStation station)
        {
            // Subtract reserved items from the maximum capacity.
            // Math.Max ensures that the result is never negative.
            return Math.Max(
                0,
                station.CapacityPerInterval - ReservedQuantity
            );
        }
    }
}
