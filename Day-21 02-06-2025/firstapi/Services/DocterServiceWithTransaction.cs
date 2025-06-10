namespace FirstApi.Services;

using FirstApi.Interfaces;
using FirstApi.Models;
using FirstApi.Models.DTOs.DoctorSpecialities;
using FirstApi.Misc;
using FirstApi.Contexts;
using FirstApi.Repositories;
using Microsoft.EntityFrameworkCore;

public class DoctorServiceWithTransaction : IDoctorService
{
    private readonly ClinicContext _clinicContext;
    private readonly DoctorMapper _doctorMapper;
    private readonly SpecialityMapper _specialityMapper;

    public DoctorServiceWithTransaction(ClinicContext clinicContext)
    {
        _clinicContext = clinicContext;
        _doctorMapper = new DoctorMapper();
        _specialityMapper = new SpecialityMapper();

    }


    //implement transaction here
    public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctorDto)
    {
        using var transaction = await _clinicContext.Database.BeginTransactionAsync();
        var doctor = _doctorMapper.MapDoctorAddRequestDto(doctorDto);

        try
        {
            await _clinicContext.AddAsync(doctor);
            await _clinicContext.SaveChangesAsync();
            if (doctorDto.Specialities != null)
            {
                foreach (var specDto in doctorDto.Specialities)
                {
                    // var allSpecs = await _specialityRepository.GetAll();
                    var speciality =   _clinicContext.specialities.FirstOrDefault(s => s.Name == specDto.Name);

                    if (speciality == null)
                    {
                        speciality =  _specialityMapper.MapSpecialityAddRequestDto(specDto);
                        await _clinicContext.AddAsync(speciality);
                        await _clinicContext.SaveChangesAsync();
                    }

                    var doctorSpeciality = _specialityMapper.MapDoctorSpeciality(doctor.Id, speciality.Id);
                    await _clinicContext.AddAsync(doctorSpeciality);
                }
                await _clinicContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return doctor;
            }

        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw new Exception("An error occurred while adding the doctor and their specialities.", e);
        }
        return null;

    }

    public Task<ICollection<DoctorsByNameResponseDto>> GetDoctByName(string name)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string speciality)
    {
        throw new NotImplementedException();
    }
    
    public Task<IEnumerable<Doctor>> GetAllDoctors()
    {
        return _clinicContext.doctors.ToListAsync().ContinueWith(task => task.Result.AsEnumerable());
    }
}