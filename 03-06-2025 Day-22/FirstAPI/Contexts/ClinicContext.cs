namespace FirstApi.Contexts;

using FirstApi.Models;
using FirstApi.Models.DTOs.DoctorSpecialities;
using Microsoft.EntityFrameworkCore;
public class ClinicContext : DbContext
{
    public ClinicContext(DbContextOptions<ClinicContext> options) : base(options)
    {
    }
    public DbSet<Patient> patients { get; set; }
    public DbSet<Doctor> doctors { get; set; }
    public DbSet<DoctorSpeciality> doctorSpecialities { get; set; }
    public DbSet<Speciality> specialities { get; set; }
    public DbSet<Appointment> appointments { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<DoctorsBySpecialityResponseDto> DoctorsBySpeciality { get; set; }
    public DbSet<DoctorsByNameResponseDto> DoctorsByName { get; set; }

    public async Task<List<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string speciality)
    {
        return await this.Set<DoctorsBySpecialityResponseDto>()
                    .FromSqlInterpolated($"select * from proc_GetDoctorsBySpeciality({speciality})")
                    .ToListAsync();
    }
    public async Task<List<DoctorsByNameResponseDto>> GetDoctorsByName(string name)
    {
        return await this.Set<DoctorsByNameResponseDto>()
                    .FromSqlInterpolated($"select * from proc_GetDoctorsByName({name})")
                    .ToListAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

       modelBuilder.Entity<Patient>().HasOne(p => p.User)
                                        .WithOne(u => u.Patient)
                                        .HasForeignKey<Patient>(p => p.Email)
                                        .HasConstraintName("FK_User_Patient")
                                        .OnDelete(DeleteBehavior.Restrict);
                            
            modelBuilder.Entity<Doctor>().HasOne(p => p.User)
                                        .WithOne(u => u.Doctor)
                                        .HasForeignKey<Doctor>(p => p.Email)
                                        .HasConstraintName("FK_User_Doctor")
                                        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>().HasKey(app => app.AppointmentNumber).HasName("PK_AppointmentNumber");

        modelBuilder.Entity<Appointment>().HasOne(app => app.Patient)
                                          .WithMany(p => p.Appointments)
                                          .HasForeignKey(app => app.PatientId)
                                          .HasConstraintName("FK_Appointment_Patient")
                                          .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>().HasOne(app => app.Doctor)
                                          .WithMany(d => d.Appointments)
                                          .HasForeignKey(app => app.DoctorId)
                                          .HasConstraintName("FK_Appointment_Doctor")
                                          .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DoctorSpeciality>().HasKey(ds => ds.SerialNumber);

        modelBuilder.Entity<DoctorSpeciality>().HasOne(ds => ds.Doctor)
                                               .WithMany(d => d.DoctorSpecialities)
                                               .HasForeignKey(ds => ds.DoctorId)
                                               .HasConstraintName("FK_Speciality_Doctor")
                                               .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DoctorSpeciality>().HasOne(ds => ds.Speciality)
                                               .WithMany(s => s.DoctorSpecialities)
                                               .HasForeignKey(ds => ds.SpecialityId)
                                               .HasConstraintName("FK_Speciality_Spec")
                                               .OnDelete(DeleteBehavior.Restrict);

    }


}