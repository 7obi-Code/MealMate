
using BusinessLogic.Interfaces;
using DataAccess.Model;

namespace BusinessLogic.Services
{
    public class PickupTimeService : IFPickupTimeService
    {
        // Beregner hvor mange retter der kan placeres
        // i et bestemt tidsrum.
        public int CalculateQuantityToAssign(
            KitchenStation station,
            StationCapacitySlot slot,
            int requestedQuantity)
        {
            if (requestedQuantity <= 0)
                return 0;

            int availableCapacity =
                slot.GetAvailableCapacity(station);

            return Math.Min(requestedQuantity, availableCapacity);
        }

        // Fordeler retter på stationens tidsrum.
        public Dictionary<DateTime, int> CalculateSlotAssignment(
            KitchenStation station,
            int requestedQuantity,
            DateTime? earliestStartTime = null)
        {
            if (station == null)
                throw new ArgumentNullException(nameof(station));

            if (requestedQuantity < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(requestedQuantity));

            var assignment = new Dictionary<DateTime, int>();

            int remainingQuantity = requestedQuantity;

            foreach (var slot in station.CapacitySlots
                         .Where(s => earliestStartTime == null ||
                                     s.StartTime >= earliestStartTime.Value)
                         .OrderBy(s => s.StartTime))
            {
                if (remainingQuantity <= 0)
                    break;

                int quantityToAssign = CalculateQuantityToAssign(
                    station,
                    slot,
                    remainingQuantity);

                if (quantityToAssign <= 0)
                    continue;

                assignment[slot.StartTime] = quantityToAssign;

                remainingQuantity -= quantityToAssign;
            }

            if (remainingQuantity > 0)
            {
                throw new InvalidOperationException(
                    $"Køkkenstationen '{station.Name}' har ikke nok kapacitet. " +
                    $"{remainingQuantity} retter kunne ikke fordeles.");
            }

            return assignment;
        }

        // Beregner hvornår hele ordren forventes at være klar.
        public DateTime CalculateEstimatedReadyTime(
            Order order,
            KitchenStation station)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (station == null)
                throw new ArgumentNullException(nameof(station));

            if (order.OrderLines == null || !order.OrderLines.Any())
                return order.CreatedAt;

            if (order.OrderLines.Any(line => line.Quantity <= 0))
            {
                throw new InvalidOperationException(
                    "Ordren indeholder en ugyldig mængde.");
            }

            // Beregn det samlede antal retter.
            int totalQuantity = order.OrderLines.Sum(
                line => line.Quantity);

            // Find den længste tilberedningstid blandt retterne.
            int longestPreparationTime = order.OrderLines
                .Max(line => Math.Max(
                    0,
                    line.FoodItem.PreparationMinutes));

            // Fordel retterne på stationens ledige tidsrum.
            var assignment = CalculateSlotAssignment(
                station,
                totalQuantity,
                order.CreatedAt);

            // Find det sidste tidsrum, der er blevet tildelt.
            DateTime lastAssignedSlot = assignment.Keys.Max();

            // Beregn hvornår dette tidsrum slutter.
            DateTime capacityReadyTime = lastAssignedSlot.AddMinutes(
                station.IntervalMinutes);

            // Beregn tilberedningstiden fra ordrens oprettelse.
            DateTime preparationReadyTime = order.CreatedAt.AddMinutes(
                longestPreparationTime);

            // Hele ordren er først klar, når begge beregninger er færdige.
            return capacityReadyTime > preparationReadyTime
                ? capacityReadyTime
                : preparationReadyTime;
        }
    }
}
