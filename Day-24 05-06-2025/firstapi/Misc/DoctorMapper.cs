namespace FirstApi.Misc;
using FirstApi.Interfaces;
using FirstApi.Models;

using FirstApi.Models.DTOs.DoctorSpecialities;
public class DoctorMapper
{
    public Doctor? MapDoctorAddRequestDto(DoctorAddRequestDto addRequestDto)
    {
        Doctor doctor = new();
        doctor.Name = addRequestDto.Name;
        doctor.YearsOfExperience = addRequestDto.YearsOfExperience;
        doctor.Status = "Active"; 
        doctor.Email = addRequestDto.Email;
        return doctor;
    }
}