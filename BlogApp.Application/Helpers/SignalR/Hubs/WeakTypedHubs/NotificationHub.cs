using Microsoft.AspNetCore.SignalR;

namespace BlogApp.Application.Helpers.SignalR.Hubs.WeakTypedHubs
{
    // Weak Type hub
    public class NotificationHub : Hub
    {
        private const string _allClientsGroup = "AllClients";

        // This method is required for when client calls (from react) instead of the backend Controllers
        // Backend Controller uses IHubContext with interface so it does not uses the method below.
        public async Task SendMessage(string user, string message)
        {
            // log or save message to database here

            // Notify all connected clients
            await Clients.All.SendAsync(_allClientsGroup, user, message);
        }
    }
}
