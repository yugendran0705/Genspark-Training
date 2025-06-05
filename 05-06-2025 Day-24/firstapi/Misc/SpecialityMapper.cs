namespace FirstApi.Misc;

using FirstApi.Interfaces;
using FirstApi.Models;
using FirstApi.Models.DTOs.DoctorSpecialities;
public class SpecialityMapper
{
    public Speciality? MapSpecialityAddRequestDto(SpecialityAddRequestDto addRequestDto)
    {
        Speciality speciality = new();
        speciality.Name = addRequestDto.Name;
        speciality.Status = "Active"; 
        return speciality;
    }

    public DoctorSpeciality MapDoctorSpeciality(int doctorId, int specialityId)
    {
        DoctorSpeciality doctorSpeciality = new();
        doctorSpeciality.DoctorId = doctorId;
        doctorSpeciality.SpecialityId = specialityId;
        return doctorSpeciality;
    }
}