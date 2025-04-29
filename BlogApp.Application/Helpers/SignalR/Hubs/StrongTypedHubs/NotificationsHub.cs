using BlogApp.Application.Helpers.SignalR.Hubs.StrongTypedHubs.Interface;
using Microsoft.AspNetCore.SignalR;

namespace BlogApp.Application.Helpers.SignalR.Hubs.StrongTypedHubs
{
    public class NotificationsHub : Hub<INotificationClient>
    {
        // This method is required for when client calls (from react) instead of the backend Controllers
        // Backend Controller uses IHubContext with interface so it does not uses the method below.
        public async Task SendMessage(string user, string message)
        {
            // Instead of doing SendAsync and putting the name in the param like in weak typed hub,
            // you directly call the name from the interface.
            await Clients.All.ReceiveNotification(user, message);
        }
    }
}
