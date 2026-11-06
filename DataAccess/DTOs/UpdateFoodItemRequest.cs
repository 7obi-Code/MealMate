namespace DataAccess.DTOs
{
    public class UpdateFoodItemRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Amount { get; set; }
    }
}
