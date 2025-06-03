namespace FirstApi.Repositories;

using FirstApi.Interfaces;
using FirstApi.Contexts;
using FirstApi.Models;
using Microsoft.EntityFrameworkCore;
public class DoctorSpecialityRepository : Repository<int, DoctorSpeciality>
{
    public DoctorSpecialityRepository(ClinicContext clinicContext) : base(clinicContext)
    {
    }

    public override async Task<DoctorSpeciality> Get(int key)
    {
        var doctorspec = await _clinicContext.doctorSpecialities.SingleOrDefaultAsync(p => p.SerialNumber== key);

        return doctorspec ?? throw new Exception("No doctorSpeciality with the given ID");
    }

    public override async Task<IEnumerable<DoctorSpeciality>> GetAll()
    {
        var doctorsspec = _clinicContext.doctorSpecialities;
        if (doctorsspec.Count() == 0)
            throw new Exception("No doctorSpeciality in the database");
        return (await doctorsspec.ToListAsync());
    }

    
}