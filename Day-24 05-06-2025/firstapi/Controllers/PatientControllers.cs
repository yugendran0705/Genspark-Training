using Microsoft.AspNetCore.Mvc;
using FirstApi.Models;
using FirstApi.Interfaces;
using FirstApi.Models.DTOs.DoctorSpecialities;
using FirstApi.Models.DTOs.Patient;

using FirstApi.Services;
using FirstApi.Repositories;
using FirstApi.Contexts;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]

public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpPost("CreatePatient")]
    public async Task<ActionResult<Patient>> CreatePatient(PatientAddDto patientDto)
    {
        try
        {
            var patient = await _patientService.AddPatient(patientDto);
            return Created(" ", patient);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("GetAllPatients")]
    [Authorize(Roles = "Patient")]
    public async Task<ActionResult<IEnumerable<Patient>>> GetAllPatients()
    {
        try
        {
            var patients = await _patientService.GetAllPatients();
            return Ok(patients);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}