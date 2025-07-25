using Microsoft.EntityFrameworkCore;
using VehicleServiceAPI.Models;

namespace VehicleServiceAPI.Context
{
    public class VehicleServiceDbContext : DbContext
    {
        public VehicleServiceDbContext(DbContextOptions<VehicleServiceDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ServiceSlot> ServiceSlots { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "Admin" },
                new Role { Id = 2, RoleName = "Mechanic" },
                new Role { Id = 3, RoleName = "User" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
