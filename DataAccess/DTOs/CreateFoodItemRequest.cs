namespace DataAccess.DTOs
{
    public class CreateFoodItemRequest
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsAvailable { get; set; } = true;
        public decimal Amount { get; set; }
    }
}
