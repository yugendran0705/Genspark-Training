namespace FirstApi.Models.DTOs;

using System.ComponentModel.DataAnnotations;
public class AppointmentDto
{
    public int Id { get; set; }
    
   
    public int PatientId { get; set; }
    
  
    public int DoctorId { get; set; }
}