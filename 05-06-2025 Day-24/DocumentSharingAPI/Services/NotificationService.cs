using System.Threading.Tasks;
using DocumentSharingAPI.Interfaces;
using DocumentSharingAPI.Models;
using Microsoft.AspNetCore.SignalR;
using DocumentSharingAPI.Hubs;

namespace DocumentSharingAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        
        public async Task NotifyDocumentUploadAsync(Document document)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveDocumentUpload", new
            {
                document.FileName,
                document.UploadedAt,
                Message = "A new document has been uploaded."
            });
        }
    }
}
