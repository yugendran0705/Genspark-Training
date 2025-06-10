using FirstApi.Contexts;
using FirstApi.Models.DTOs;
using FirstApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstApi.Repositories
{
    public class DoctorRepository : Repository<int, Doctor>
    {
        protected DoctorRepository(ClinicContext clinicContext) : base(clinicContext)
        {
        }

        public override async Task<Doctor> Get(int key)
        {
            var doctor = await _clinicContext.Doctors.SingleOrDefaultAsync(p => p.Id == key);

            return doctor ?? throw new Exception("No doctor with teh given ID");
        }

        public override async Task<IEnumerable<Doctor>> GetAll()
        {
            var doctors = _clinicContext.Doctors;
            if (doctors.Count() == 0)
                throw new Exception("No Doctor in the database");
            return await doctors.ToListAsync();
        }
        
        public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctorDto)
        {
            var doctor = new Doctor
            {
                Name = doctorDto.Name,
                YearsOfExperience = doctorDto.YearsOfExperience,
                DoctorSpecialities = new List<DoctorSpeciality>()
            };

            if (doctorDto.SpecialityIds != null)
            {
                foreach (var specialityId in doctorDto.SpecialityIds)
                {
                    var speciality = await _clinicContext.Specialities.FindAsync(specialityId) ?? throw new Exception($"Speciality with ID {specialityId} not found");
                    doctor.DoctorSpecialities.Add(new DoctorSpeciality
                    {
                        Doctor = doctor,
                        Speciality = speciality
                    });
                }
            }

            await _clinicContext.Doctors.AddAsync(doctor);
            await _clinicContext.SaveChangesAsync();
            return doctor;
        }

        public async Task<bool> DeleteDoctor(int key)
        {
            var doctor = await Get(key);
            _clinicContext.Doctors.Remove(doctor);
            await _clinicContext.SaveChangesAsync();
            return true;
        }

        public async Task<Doctor> UpdateDoctor(int id, Doctor updatedDoctor)
        {
            var existingDoctor = await Get(id) ?? throw new InvalidOperationException("Doctor not found.");
            existingDoctor.Name = updatedDoctor.Name;
            existingDoctor.YearsOfExperience = updatedDoctor.YearsOfExperience;
            existingDoctor.Status = updatedDoctor.Status;

            _clinicContext.Doctors.Update(existingDoctor);
            await _clinicContext.SaveChangesAsync();
            return existingDoctor;
        }
    }
}