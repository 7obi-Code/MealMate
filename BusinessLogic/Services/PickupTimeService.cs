
using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Interfaces;
using DataAccess.Model;

namespace BusinessLogic.Services
{
    // Handles the business logic for calculating pickup times
    // and assigning food items to available kitchen time slots.
    public class PickupTimeService : IFPickupTimeService
    {
        // Calculates when an order is expected to be ready.
        public DateTime CalculateEstimatedReadyTime(Order order)
        {
            // TODO: Calculate the estimated pickup time based on
            // kitchen station capacity and available time slots.
            throw new NotImplementedException();
        }

        // Calculates how many food items can be assigned
        // to a specific kitchen station time slot.
        public int CalculateQuantityToAssign(
            KitchenStation station,
            StationCapacitySlot slot,
            int requestedQuantity)
        {
            // Get the remaining capacity in the current time slot.
            int availableCapacity = slot.GetAvailableCapacity(station);

            // Return the smallest value to ensure that we do not
            // assign more items than requested or available.
            return Math.Min(requestedQuantity, availableCapacity);
        }

        // Calculates how food items can be assigned
        // across the available time slots of a kitchen station.
        public Dictionary<DateTime, int> CalculateSlotAssignment(
            KitchenStation station,
            int requestedQuantity)
        {
            // Stores the quantity of food items assigned to each time slot.
            var assignment = new Dictionary<DateTime, int>();

            // Tracks how many food items still need to be assigned.
            int remainingQuantity = requestedQuantity;

            // Process the time slots in chronological order.
            foreach (var slot in station.CapacitySlots.OrderBy(s => s.StartTime))
            {
                // Stop when all requested food items have been assigned.
                if (remainingQuantity <= 0)
                    break;

                // Calculate how many items can be assigned to this slot.
                int quantityToAssign = CalculateQuantityToAssign(
                    station,
                    slot,
                    remainingQuantity
                );

                // Skip this slot if no capacity is available.
                if (quantityToAssign == 0)
                    continue;

                // Store the quantity assigned to this time slot.
                assignment[slot.StartTime] = quantityToAssign;

                // Subtract the assigned quantity from the remaining items.
                remainingQuantity -= quantityToAssign;
            }

            // Return the calculated assignment across existing time slots.
            return assignment;
        }
    }
}
