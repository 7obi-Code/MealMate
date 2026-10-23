using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Model;

public class OrderLine
{
    public int OrderLineId { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int FoodItemId { get; set; }
    public FoodItem FoodItem { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal { get; set; }
}

