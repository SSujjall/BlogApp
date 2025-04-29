using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Helpers.SignalR.Hubs.StrongTypedHubs.Interface
{
    public interface INotificationClient
    {
        Task ReceiveNotification(string user, string message);
    }
}
