using BlogApp.Domain.Enums;

namespace BlogApp.Application.DTOs
{
    public class CreateOrderDTO
    {
        public int SubscriptionId { get; set; }
    }

    public class UpdateOrderDTO
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
