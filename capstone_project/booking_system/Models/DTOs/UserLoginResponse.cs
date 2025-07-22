namespace BookingSystem.Models.DTOs;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserLoginResponse
{
    public string Username { get; set; } = string.Empty;
    public string? Token { get; set; }
    public string? Role {get;set;}
    public string RefreshToken { get; set; } 
}