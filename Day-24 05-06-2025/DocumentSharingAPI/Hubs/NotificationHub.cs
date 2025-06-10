using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DocumentSharingAPI.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            // Log the connection ID for debugging purposes
            System.Console.WriteLine($"Client connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }
        
        // Example hub method for testing client-to-server communication
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }

}