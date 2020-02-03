using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontendRequestService.Socket;
using Microsoft.AspNetCore.SignalR;

namespace FrontendRequestService
{
    public class NotifyService
    {
        private readonly IHubContext<DownloadSocket> _hub;

        public NotifyService(IHubContext<DownloadSocket> hub)
        {
            _hub = hub;
        }

        public Task SendNotificationAsync(string message)
        {
            return _hub.Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
