using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.SignalR.Hubs.StrongTypedHubs;
using BlogApp.Application.Helpers.SignalR.Hubs.StrongTypedHubs.Interface;
using BlogApp.Application.Helpers.SignalR.Hubs.WeakTypedHubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BlogApp.API.Controllers
{
    /// <summary>
    /// This class contains methods for mocking notifications.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        // Access hubs from outside the hub scope (i.e. from controller or other classes)
        private readonly IHubContext<NotificationsHub, INotificationClient> _strongHubContext;
        private readonly IHubContext<NotificationHub> _weakHubContext;

        public NotificationController(
            IHubContext<NotificationsHub, INotificationClient> strongHubContext,
            IHubContext<NotificationHub> weakHubContext)
        {
            _strongHubContext = strongHubContext;
            _weakHubContext = weakHubContext;
        }

        [HttpPost("str-send-noti")]
        public async Task<IActionResult> StrSendNotification(SendNotificationDTO model)
        {
            model.User = "AdminUser";
            await _strongHubContext.Clients.All.ReceiveNotification(model.User, model.Message);
            return Ok();
        }

        [HttpPost("weak-send-noti")]
        public async Task<IActionResult> WeakSendNotification(SendNotificationDTO model)
        {
            model.User = "AdminUser";
            await _weakHubContext.Clients.All.SendAsync("AllClients", model.User, model.Message);
            return Ok();
        }
    }
}
