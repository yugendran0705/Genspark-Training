using FirstApi.Interfaces;
using FirstApi.Models;
using FirstApi.Models.DTOs;

namespace FirstApi.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IRepository<int, Doctor> _doctorRepository;
        private readonly IRepository<int, Speciality> _specialityRepository;
        // The _doctorSpecialityRepository is available if you need extra operations on the relationship.
        private readonly IRepository<int, DoctorSpeciality> _doctorSpecialityRepository;

        public DoctorService(IRepository<int, Doctor> doctorRepository,
                             IRepository<int, Speciality> specialityRepository,
                             IRepository<int, DoctorSpeciality> doctorSpecialityRepository)
        {
            _doctorRepository = doctorRepository;
            _specialityRepository = specialityRepository;
            _doctorSpecialityRepository = doctorSpecialityRepository;
        }

        public async Task<Doctor> GetDoctorByName(string name)
        {
            // Fetch all doctors then filter in memory.
            var doctors = await _doctorRepository.GetAll();
            var doctor = doctors.FirstOrDefault(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return doctor ?? throw new Exception("No doctor with the given name");
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsBySpeciality(string speciality)
        {
            var doctors = await _doctorRepository.GetAll();
            var filteredDoctors = doctors.Where(d =>
                d.DoctorSpecialities != null &&
                d.DoctorSpecialities.Any(ds =>
                    ds.Speciality != null &&
                    ds.Speciality.Name.Equals(speciality, StringComparison.OrdinalIgnoreCase)
                )).ToList();

            if (filteredDoctors.Count == 0)
                throw new Exception("No doctors found with the given speciality");

            return filteredDoctors;
        }

        public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctorDto)
        {
            var doctor = new Doctor
            {
                Name = doctorDto.Name,
                YearsOfExperience = doctorDto.YearsOfExperience,
                // Initialize the relationship collection. Note that the Doctor model has DoctorSpecialities.
                DoctorSpecialities = new List<DoctorSpeciality>()
            };

            if (doctorDto.SpecialityIds != null)
            {
                foreach (var specialityId in doctorDto.SpecialityIds)
                {
                    var speciality = await _specialityRepository.Get(specialityId);
                    if (speciality == null)
                        throw new Exception($"Speciality with ID {specialityId} not found");

                    doctor.DoctorSpecialities.Add(new DoctorSpeciality
                    {
                        Doctor = doctor,
                        Speciality = speciality
                    });
                }
            }

            await _doctorRepository.Add(doctor);
            return doctor;
        }

        public async Task<IEnumerable<Doctor>> GetDoctors()
        {
            var doctors = await _doctorRepository.GetAll();
            if (!doctors.Any())
                throw new Exception("No doctor in the database");
            return doctors;
        }

        public async Task<Doctor> UpdateDoctor(int id, Doctor updatedDoctor)
        {
            var existingDoctor = await _doctorRepository.Get(id);
            if (existingDoctor == null)
                throw new Exception("Doctor not found");

            existingDoctor.Name = updatedDoctor.Name;
            existingDoctor.YearsOfExperience = updatedDoctor.YearsOfExperience;
            existingDoctor.Status = updatedDoctor.Status;

            await _doctorRepository.Update(id, existingDoctor);
            return existingDoctor;
        }

        public async Task<bool> DeleteDoctor(int id)
        {
            var deletedDoctor = await _doctorRepository.Delete(id);
            return deletedDoctor != null;
        }
    }
}
