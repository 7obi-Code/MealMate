
using NUnit.Framework;
using BusinessLogic.Services;
using DataAccess.Model;
using System;

namespace BusinessLogic.Tests
{
    [TestFixture]
    public class PickupTimeServiceTests
    {
        [Test]
        public void CalculateSlotAssignment_ShouldAssign40BurgersAcrossThreeSlots()
        {
            // Arrange

            // Create a kitchen station with a capacity of
            // 25 burgers per 15-minute interval.
            var station = new KitchenStation
            {
                Name = "Burger Station",
                CapacityPerInterval = 25,
                IntervalMinutes = 15
            };

            // Define the start time for the first slot.
            var startTime = new DateTime(2026, 10, 29, 12, 0, 0);

            // Create three time slots.
            // The first slot already has 20 reserved burgers.
            station.CapacitySlots.Add(new StationCapacitySlot
            {
                StartTime = startTime,
                ReservedQuantity = 20
            });

            station.CapacitySlots.Add(new StationCapacitySlot
            {
                StartTime = startTime.AddMinutes(15),
                ReservedQuantity = 0
            });

            station.CapacitySlots.Add(new StationCapacitySlot
            {
                StartTime = startTime.AddMinutes(30),
                ReservedQuantity = 0
            });

            // Create an instance of the service being tested.
            var service = new PickupTimeService();

            // Act

            // Calculate how 40 burgers can be assigned
            // across the available time slots.
            var assignment = service.CalculateSlotAssignment(station, 40);

            // Assert

            // Verify that the order is assigned across three slots.
            Assert.That(assignment.Count, Is.EqualTo(3));

            // The first slot has room for 5 burgers.
            Assert.That(assignment[startTime], Is.EqualTo(5));

            // The second slot has room for 25 burgers.
            Assert.That(assignment[startTime.AddMinutes(15)], Is.EqualTo(25));

            // The remaining 10 burgers go into the third slot.
            Assert.That(assignment[startTime.AddMinutes(30)], Is.EqualTo(10));
        }
    }
}
