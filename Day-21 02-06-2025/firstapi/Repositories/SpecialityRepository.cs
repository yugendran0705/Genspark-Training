namespace FirstApi.Repositories;

using FirstApi.Interfaces;
using FirstApi.Contexts;
using FirstApi.Models;
using Microsoft.EntityFrameworkCore;
public class SpecialityRepository : Repository<int, Speciality>
{
    public SpecialityRepository(ClinicContext clinicContext) : base(clinicContext)
    {
    }

    public override async Task<Speciality> Get(int key)
    {
        var doctorspec = await _clinicContext.specialities.SingleOrDefaultAsync(p => p.Id == key);

        return doctorspec ;
    }

    public override async Task<IEnumerable<Speciality>> GetAll()
    {
        var doctorsspec = _clinicContext.specialities;
        // if (doctorsspec.Count() == 0)
        //     throw new Exception("No speciality in the database");
        return (await doctorsspec.ToListAsync());
    }

    
}