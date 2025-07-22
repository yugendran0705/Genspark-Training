namespace BookingSystem.Models.DTOs;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserLoginRequest
{
    [Required(ErrorMessage = "Username is manditory")]
    [MinLength(2, ErrorMessage = "Invalid entry for username")]
    public string Username { get; set; } = string.Empty;
    [Required(ErrorMessage = "Password is manditory")]
    public string Password { get; set; } = string.Empty;
}