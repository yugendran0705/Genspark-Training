namespace FirstApi.Services;

using FirstApi.Interfaces;
using FirstApi.Models;
using FirstApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using FirstApi.Repositories;
public class AppointmentService : IAppointmentService
{
    private readonly IRepository<string,Appointment> _appointmentRepository;
    public AppointmentService(IRepository<string,Appointment> appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }
    public async Task<Appointment> CreateAppointmentAsync(AppointmentDto appointmentDto)
    {
        

        var appointment = new Appointment
        {
            AppointmentNumber = Guid.NewGuid().ToString(),
            PatientId = appointmentDto.PatientId,
            DoctorId = appointmentDto.DoctorId,
            AppointmentDateTime = DateTime.UtcNow,
            Status = "Scheduled" 
        };

        return await _appointmentRepository.Add(appointment);
    }
}