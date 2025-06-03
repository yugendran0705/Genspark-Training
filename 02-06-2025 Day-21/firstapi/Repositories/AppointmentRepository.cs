namespace FirstApi.Repositories;

using FirstApi.Interfaces;
using FirstApi.Contexts;
using FirstApi.Models;
using Microsoft.EntityFrameworkCore;
public class AppointmentRepository : Repository<string, Appointment>
{
    public AppointmentRepository(ClinicContext clinicContext) : base(clinicContext)
    {
    }

    public override async Task<Appointment> Get(string key)
    {
        var doctorspec = await _clinicContext.appointments.SingleOrDefaultAsync(p => p.AppointmentNumber== key.ToString());

        return doctorspec ?? throw new Exception("No appointment with the given ID");
    }

    public override async Task<IEnumerable<Appointment>> GetAll()
    {
        var doctorsspec = _clinicContext.appointments;
        if (doctorsspec.Count() == 0)
            throw new Exception("No appointments in the database");
        return (await doctorsspec.ToListAsync());
    }

    
}