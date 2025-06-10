using System.Threading.Tasks;
using DocumentSharingAPI.Models;

namespace DocumentSharingAPI.Interfaces
{
    public interface IDocumentRepository
    {
        Task AddDocumentAsync(Document document);
        // Other CRUD methods can be added here.
    }
}
