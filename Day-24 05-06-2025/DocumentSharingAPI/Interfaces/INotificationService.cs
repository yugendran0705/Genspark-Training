using System.Threading.Tasks;
using DocumentSharingAPI.Models;

namespace DocumentSharingAPI.Interfaces
{
    public interface INotificationService
    {
        Task NotifyDocumentUploadAsync(Document document);
    }
}
