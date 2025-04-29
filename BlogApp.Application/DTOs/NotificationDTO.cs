using System.Text.Json.Serialization;

namespace BlogApp.Application.DTOs
{
    public class SendNotificationDTO
    {
        [JsonIgnore]
        public string? User { get; set; }
        public string Message { get; set; }
    }
}
