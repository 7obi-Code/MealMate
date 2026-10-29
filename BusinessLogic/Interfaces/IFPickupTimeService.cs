using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Interfaces
{
    public interface IFPickupTimeService
    {
        // Calculates when an order is estimated to be ready for pickup based on the restaurant's kitchen stations and their capacity.
        DateTime CalculateEstimatedReadyTime(Order order);
    }
}
