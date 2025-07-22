namespace filestorage.Contexts;

using Microsoft.EntityFrameworkCore;
using filestorage.models;

public class TrainingDbContext : DbContext
{
    public TrainingDbContext(DbContextOptions<TrainingDbContext> options)
        : base(options)
    {
    }

    public DbSet<TrainingVideo> TrainingVideos { get; set; }
}
