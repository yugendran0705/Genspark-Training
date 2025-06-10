using Microsoft.AspNetCore.Mvc;
using FirstApi.Models;
using FirstApi.Models.DTOs;
using FirstApi.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class DoctorController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
    {
        var doctors = await _doctorService.GetDoctors();
        return Ok(doctors);
    }

    // An endpoint to retrieve a doctor by name.
    [HttpGet("name/{name}")]
    public async Task<ActionResult<Doctor>> GetDoctorByName(string name)
    {
        var doctor = await _doctorService.GetDoctorByName(name);
        return Ok(doctor);
    }

    // Endpoint for retrieving doctors by speciality.
    [HttpGet("speciality/{speciality}")]
    public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctorsBySpeciality(string speciality)
    {
        var doctors = await _doctorService.GetDoctorsBySpeciality(speciality);
        return Ok(doctors);
    }

    [HttpPost]
    public async Task<ActionResult<Doctor>> PostDoctor([FromBody] DoctorAddRequestDto doctorDto)
    {
        var createdDoctor = await _doctorService.AddDoctor(doctorDto);
        return CreatedAtAction(nameof(GetDoctorByName), new { name = createdDoctor.Name }, createdDoctor);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Doctor>> UpdateDoctor(int id, [FromBody] Doctor updatedDoctor)
    {
        var doctor = await _doctorService.UpdateDoctor(id, updatedDoctor);
        if (doctor == null)
        {
            return NotFound("Doctor not found");
        }
        return Ok(doctor);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteDoctor(int id)
    {
        var deleted = await _doctorService.DeleteDoctor(id);
        if (!deleted)
        {
            return NotFound("Doctor not found");
        }
        return NoContent();
    }
}
