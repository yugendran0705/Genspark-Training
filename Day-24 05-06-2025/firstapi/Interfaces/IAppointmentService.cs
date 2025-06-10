namespace FirstApi.Interfaces;
using FirstApi.Models;
using FirstApi.Models.DTOs;
public interface IAppointmentService
{
    Task<Appointment> CreateAppointmentAsync(AppointmentDto appointmentDto);
 
}