using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Domain.Entities
{
    public class Notifications
    {
        [Key]
        public int NotificationId { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public int SourceId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        // Virtual navigation property
        public virtual Users User { get; set; } = null!;
    }
}
