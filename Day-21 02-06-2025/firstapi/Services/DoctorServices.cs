namespace FirstApi.Services;

using FirstApi.Interfaces;
using FirstApi.Models;
using FirstApi.Models.DTOs.DoctorSpecialities;
using FirstApi.Misc;
using AutoMapper;


using FirstApi.Interfaces;
public class DoctorService : IDoctorService
{
    DoctorMapper _doctorMapper;
    SpecialityMapper _specialityMapper;
    private readonly IRepository<int, Doctor> _doctorRepository;
    private readonly IRepository<int, Speciality> _specialityRepository;
    private readonly IRepository<int, DoctorSpeciality> _doctorSpecialityRepository;
    private readonly IRepository<string, User> _userRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly IMapper _mapper;

    private readonly IOtherContextFunctionities _otherContextFunctionities;

    public DoctorService(
        IRepository<int, Doctor> doctorRepository,
        IRepository<int, Speciality> specialityRepository,
        IRepository<int, DoctorSpeciality> doctorSpecialityRepository,
        IOtherContextFunctionities otherContextFunctionities,
        IRepository<string, User> userRepository,
                            IEncryptionService encryptionService,
                            IMapper mapper)

    {
        _doctorRepository = doctorRepository;
        _specialityRepository = specialityRepository;
        _doctorSpecialityRepository = doctorSpecialityRepository;
        _doctorMapper = new DoctorMapper();
        _specialityMapper = new SpecialityMapper();
        _otherContextFunctionities = otherContextFunctionities;
        _userRepository = userRepository;
        _encryptionService = encryptionService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Doctor>> GetAllDoctors()
    {
        return await _doctorRepository.GetAll();
    }
    public async Task<ICollection<DoctorsByNameResponseDto>> GetDoctByName(string name)
    {
        // var allDoctors = await _doctorRepository.GetAll();
        // return allDoctors.FirstOrDefault(d => d.Name == name)
        //     ?? throw new Exception("Doctor not found");
        var result = await _otherContextFunctionities.GetDoctorsByName(name);

        return result;
    }

    public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctorDto)
    {
        var user = _mapper.Map<DoctorAddRequestDto, User>(doctorDto);
        var encryptedData = await _encryptionService.EncryptData(new EncryptModel
        {
            Data = doctorDto.Password,
        });
        user.Password = encryptedData.EncryptedData;
        user.HashKey = encryptedData.HashKey;
        user.Role = "Doctor";
        user = await _userRepository.Add(user);
        var doctor = _doctorMapper.MapDoctorAddRequestDto(doctorDto);

        var addedDoctor = await _doctorRepository.Add(doctor);

        if (doctorDto.Specialities != null)
        {
            foreach (var specDto in doctorDto.Specialities)
            {
                var allSpecs = await _specialityRepository.GetAll();
                var speciality = allSpecs.FirstOrDefault(s => s.Name == specDto.Name);

                if (speciality == null)
                {
                    speciality = _specialityMapper.MapSpecialityAddRequestDto(specDto);
                    speciality = await _specialityRepository.Add(speciality);
                }

                var doctorSpeciality = _specialityMapper.MapDoctorSpeciality(addedDoctor.Id, speciality.Id);
                await _doctorSpecialityRepository.Add(doctorSpeciality);
            }
        }

        return addedDoctor;
    }
    public async Task<ICollection<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string speciality)
    {
        var result = await _otherContextFunctionities.GetDoctorsBySpeciality(speciality);
        return result;
    }


}