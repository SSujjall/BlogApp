namespace BlogApp.Application.DTOs
{
    public class CreateSubscriptionDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateSubscriptionDTO
    {
        public int SubscriptionId { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? DurationInMonths { get; set; }
        public string? Description { get; set; }
    }
}
