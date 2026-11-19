
using DataAccess.Model;

namespace BusinessLogic.Interfaces
{
    public interface IFPickupTimeService
    {
        // Beregner forventet afhentningstid.
        DateTime CalculateEstimatedReadyTime(
            Order order,
            KitchenStation station);

        // Beregner antal retter, der kan placeres i et tidsrum.
        int CalculateQuantityToAssign(
            KitchenStation station,
            StationCapacitySlot slot,
            int requestedQuantity);

        // Fordeler retter på stationens ledige tidsrum.
        Dictionary<DateTime, int> CalculateSlotAssignment(
            KitchenStation station,
            int requestedQuantity,
            DateTime? earliestStartTime = null);
    }
}
