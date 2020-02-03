using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace FrontendRequestService.Socket
{
    public class DownloadSocket : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
