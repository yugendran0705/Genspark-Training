namespace FirstApi.Misc;

using FirstApi.Interfaces;
using FirstApi.Models;
using FirstApi.Contexts;
using FirstApi.Models.DTOs.DoctorSpecialities;

public class OtherFuncinalitiesImplementation : IOtherContextFunctionities
{
    private readonly ClinicContext _clinicContext;

    public OtherFuncinalitiesImplementation(ClinicContext clinicContext)
    {
        _clinicContext = clinicContext;
    }

    public async Task<ICollection<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string specilaity)
    {
        var result = await _clinicContext.GetDoctorsBySpeciality(specilaity);
        return result;
    }

    public async Task<ICollection<DoctorsByNameResponseDto>> GetDoctorsByName(string name)
    {
        var result = await _clinicContext.GetDoctorsByName(name);
        return result;
    }
}
