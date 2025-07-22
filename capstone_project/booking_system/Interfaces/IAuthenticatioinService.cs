namespace BookingSystem.Interfaces;

using BookingSystem.Models;
using BookingSystem.Models.DTOs;

public interface IAuthenticationService
{
    public Task<UserLoginResponse> Login(UserLoginRequest user);
    public  Task Logout(string email,string token);
    public  Task<UserLoginResponse> RefreshToken(string email, string refreshToken);

}