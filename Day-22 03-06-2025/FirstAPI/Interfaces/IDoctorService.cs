namespace FirstApi.Interfaces;

using FirstApi.Models;
using FirstApi.Models.DTOs.DoctorSpecialities;

public interface IDoctorService
{
    public Task<ICollection<DoctorsByNameResponseDto>> GetDoctByName(string name);
    // public Task<ICollection<Doctor>> GetDoctorsBySpeciality(string speciality);
    public Task<ICollection<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string speciality);
    public Task<Doctor> AddDoctor(DoctorAddRequestDto doctor);

    public Task<IEnumerable<Doctor>> GetAllDoctors();

    public Task<Appointment> DeleteApppointment(string appointmentNumber);
}
