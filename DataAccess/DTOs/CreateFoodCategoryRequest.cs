using System;
using System.Collections.Generic;
using System.Text;


namespace DataAccess.DTOs;

public class CreateFoodCategoryRequest
{
    public int MenuId { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<int> FoodItemIds { get; set; } = new();
}

