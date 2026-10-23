using System;
using System.Collections.Generic;
using System.Text;


namespace DataAccess.Model;

public class Order
{
    public int OrderId { get; set; }

    public string OrderType { get; set; } = "";
    public string OrderStatus { get; set; } = "Cart";

    public DateTime CreatedAt { get; set; }
    public DateTime? RequestedPickupTime { get; set; }
    public DateTime? EstimatedReadyTime { get; set; }

    public decimal Total { get; set; }

    public List<OrderLine> OrderLines { get; set; } = new();
}

