namespace FirstApi.Repositories;

using FirstApi.Interfaces;
using FirstApi.Contexts;
using FirstApi.Models;
using Microsoft.EntityFrameworkCore;
public class PatientRepository : Repository<int, Patient>
{
    public PatientRepository(ClinicContext clinicContext) : base(clinicContext)
    {
    }

    public override async Task<Patient> Get(int key)
    {
        var patient = await _clinicContext.patients.SingleOrDefaultAsync(p => p.Id == key);

        return patient ?? throw new Exception("No patient with the given ID");
    }

    public override async Task<IEnumerable<Patient>> GetAll()
    {
        var patients = _clinicContext.patients;
        if (patients.Count() == 0)
            throw new Exception("No Patients in the database");
        return (await patients.ToListAsync());
    }

    
}