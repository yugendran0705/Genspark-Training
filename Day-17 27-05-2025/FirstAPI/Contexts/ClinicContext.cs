namespace FirstApi.Contexts;
using FirstApi.Models;
using Microsoft.EntityFrameworkCore;
public class ClinicContext : DbContext
{
    public ClinicContext(DbContextOptions<ClinicContext> options) : base(options)
    {
    }
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseNpgsql("User ID=postgres;Password=password;Host=localhost;Port=5432;Database=myDataBase;");
    // }
    public DbSet<Patient> patients { get; set; }
    
}