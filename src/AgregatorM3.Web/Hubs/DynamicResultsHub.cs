using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AgregatorM3.Web.Hubs
{
    public class DynamicResultsHub : Hub
    {
        public async Task SendMessage(string serviceName, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", serviceName, message);
        }
    }
}
