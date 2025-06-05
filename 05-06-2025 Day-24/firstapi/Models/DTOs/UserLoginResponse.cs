namespace FirstApi.Models.DTOs.DoctorSpecialities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserLoginResponse
{
    public string Username { get; set; } = string.Empty;
    public string? Token { get; set; }
}