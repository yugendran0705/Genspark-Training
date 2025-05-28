using FirstApi.Models;
using FirstApi.Models.DTOs;

namespace FirstApi.Interfaces
{
    public interface IDoctorService
    {
        Task<Doctor> GetDoctorByName(string name);
        Task<IEnumerable<Doctor>> GetDoctorsBySpeciality(string speciality);
        Task<Doctor> AddDoctor(DoctorAddRequestDto doctorDto);
        Task<IEnumerable<Doctor>> GetDoctors();
        Task<Doctor> UpdateDoctor(int id, Doctor updatedDoctor);
        Task<bool> DeleteDoctor(int id);
    }
}
