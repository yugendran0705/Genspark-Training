using Microsoft.AspNetCore.Mvc;
using FirstApi.Models;
using FirstApi.Interfaces;
using FirstApi.Models.DTOs.DoctorSpecialities;
using FirstApi.Services;
using FirstApi.Repositories;
using FirstApi.Contexts;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class DoctorController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }


    [HttpGet("GetAllDoctors")]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<ActionResult<IEnumerable<Doctor>>> GetAllDoctors()
    {
        try
        {
            var doctors = await _doctorService.GetAllDoctors();
            return Ok(doctors);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
    [Authorize(Policy = "ExperiencedDoctorOnly")]
    [HttpDelete("DeleteAppointment/{id}")]
    public async Task<ActionResult> DeleteAppointment(string id)
    {
        try
        {
            await _doctorService.DeleteApppointment(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetDoctorByName/{name}")]
    public async Task<ActionResult<Doctor>> GetDoctorByName(string name)
    {
        try
        {
            var doctor = await _doctorService.GetDoctByName(name);
            return Ok(doctor);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("GetDoctorsBySpeciality/{speciality}")]
    public async Task<ActionResult<ICollection<Doctor>>> GetDoctorsBySpeciality(string speciality)
    {
        try
        {
            var doctors = await _doctorService.GetDoctorsBySpeciality(speciality);
            return Ok(doctors);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("AddDoctor")]
    public async Task<ActionResult<Doctor>> AddDoctor([FromBody] DoctorAddRequestDto doctorDto)
    {
        try
        {
            var doctor = await _doctorService.AddDoctor(doctorDto);
            return Created("",doctor);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}