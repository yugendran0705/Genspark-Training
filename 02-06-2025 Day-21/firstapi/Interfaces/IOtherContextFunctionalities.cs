namespace FirstApi.Interfaces;

using FirstApi.Models.DTOs.DoctorSpecialities;
using FirstApi.Models;

public interface IOtherContextFunctionities
{
    public Task<ICollection<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string specilaity);

    public Task<ICollection<DoctorsByNameResponseDto>> GetDoctorsByName(string name);
}