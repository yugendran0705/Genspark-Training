using System.Threading.Tasks;
using DocumentSharingAPI.Interfaces;
using DocumentSharingAPI.Models;
using DocumentSharingAPI.Context;

namespace DocumentSharingAPI.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDbContext _context;
        public DocumentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task AddDocumentAsync(Document document)
        {
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();
        }
    }
}
