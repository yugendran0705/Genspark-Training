namespace FirstApi.Services;

using FirstApi.Interfaces;
using FirstApi.Models;
using FirstApi.Models.DTOs.Patient;
using FirstApi.Repositories;
using FirstApi.Misc;

public class PatientService : IPatientService
{
    private readonly IRepository<int, Patient> _patientRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly IRepository<string, User> _userRepository;

    public PatientService(IRepository<int,Patient> patientRepository , IEncryptionService encryptionService, IRepository<string, User> userRepository)
    {
        _patientRepository = patientRepository;
        _encryptionService = encryptionService;
        _userRepository = userRepository;
    }

    public async Task<Patient> AddPatient(PatientAddDto addRequestDto)
    {

        try
        {
            var patient = new Patient
            {
                Name = addRequestDto.Name,
                Age = addRequestDto.Age,
                Email = addRequestDto.Email,
                Phone = addRequestDto.Phone,
            };
            var encryptedData = await _encryptionService.EncryptData(new EncryptModel
            {
                Data = addRequestDto.Password
            });
            var user= new User
            {
                Username = addRequestDto.Email,
                Role = "Patient",
                Password = encryptedData.EncryptedData,
                HashKey = encryptedData.HashKey,

            };
            user = await _userRepository.Add(user);
            var addedpatient = await _patientRepository.Add(patient);
            if (addedpatient == null)
            {
                throw new Exception("Failed to add patient");
            }
            return addedpatient;
        }
        catch (Exception e)
        {
            throw new Exception($"Error adding patient: {e.Message}");
        }


    }
    public async Task<IEnumerable<Patient>> GetAllPatients()
    {
        return await _patientRepository.GetAll();
    }
}


