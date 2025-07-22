namespace BookingSystem.Contexts;

using Microsoft.EntityFrameworkCore;
using BookingSystem.Models;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Wallet> Wallets { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<User>(entity =>
            {
                  entity.HasKey(u => u.Email);
                  entity.Property(u => u.Email).IsRequired();
                  entity.Property(u => u.Role).IsRequired();

                  entity.HasOne(u => u.Admin)
                    .WithOne(a => a.User)
                    .HasForeignKey<Admin>(a => a.Email);

                  entity.HasOne(u => u.Customer)
                    .WithOne(c => c.User)
                    .HasForeignKey<Customer>(c => c.Email);
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                  entity.HasKey(a => a.Email);
                  entity.Property(a => a.Name).IsRequired();

                  entity.HasMany(a => a.CreatedEvents)
                    .WithOne(e => e.CreatedBy)
                    .HasForeignKey(e => e.CreatorEmail)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                  entity.HasKey(c => c.Email);
                  entity.Property(c => c.Name).IsRequired();

                  entity.HasMany(c => c.Tickets)
                    .WithOne(t => t.User)
                    .HasForeignKey(t => t.CustomerEmail)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                  entity.HasKey(c => c.Id);
                  entity.Property(c => c.Name).IsRequired();

                  entity.HasMany(c => c.Events)
                    .WithOne(e => e.Category)
                    .HasForeignKey(e => e.CategoryId);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                  entity.HasKey(e => e.Id);
                  entity.Property(e => e.Title).IsRequired();
                  entity.Property(e => e.Description).IsRequired();

                  entity.HasOne(e => e.Category)
                    .WithMany(c => c.Events)
                    .HasForeignKey(e => e.CategoryId);

                  entity.HasOne(e => e.CreatedBy)
                    .WithMany(a => a.CreatedEvents)
                    .HasForeignKey(e => e.CreatorEmail)
                    .OnDelete(DeleteBehavior.Restrict);

                  entity.HasMany(e => e.Tickets)
                    .WithOne(t => t.Event)
                    .HasForeignKey(t => t.EventId);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                  entity.HasKey(t => t.Id);

                  entity.Property(t => t.BookingDate).IsRequired();

                  entity.HasOne(t => t.Event)
                    .WithMany(e => e.Tickets)
                    .HasForeignKey(t => t.EventId);

                  entity.HasOne(t => t.User)
                    .WithMany(c => c.Tickets)
                    .HasForeignKey(t => t.CustomerEmail);
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                  entity.HasKey(w => w.CustomerEmail);

                  entity.Property(w => w.balance).IsRequired();

                  entity.Property(w => w.LastUpdated)
                        .IsRequired()
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                  entity.HasOne(w => w.Customer)
                        .WithOne(c => c.Wallet)
                        .HasForeignKey<Wallet>(w => w.CustomerEmail)
                        .OnDelete(DeleteBehavior.Cascade);
            });
      }

}
