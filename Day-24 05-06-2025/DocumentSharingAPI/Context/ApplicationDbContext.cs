using Microsoft.EntityFrameworkCore;
using DocumentSharingAPI.Models;

namespace DocumentSharingAPI.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
    }
}
