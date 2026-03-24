using BlogApp.Domain.Enums;

namespace BlogApp.Application.DTOs
{
    public class CreateOrderDTO
    {
        public int SubscriptionId { get; set; }
    }

    public class CancelOrderDto
    {
        public int OrderId { get; set; }
    }

    public class InternalOrderCancellationResultDTO
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
    }
}
