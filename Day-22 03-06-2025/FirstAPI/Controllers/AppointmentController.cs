using Microsoft.AspNetCore.Mvc;
using FirstApi.Models;
using FirstApi.Interfaces;
using FirstApi.Models.DTOs.DoctorSpecialities;
using FirstApi.Services;
using FirstApi.Repositories;
using FirstApi.Contexts;
using Microsoft.AspNetCore.Authorization;
using FirstApi.Models.DTOs;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpPost("CreateAppointment")]
    public async Task<ActionResult<Appointment>> AddAppointment(AppointmentDto appointmentDto)
    {
        try
        {
            var appointment = await _appointmentService.CreateAppointmentAsync(appointmentDto);
            return Created(" ", appointment);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    
}
