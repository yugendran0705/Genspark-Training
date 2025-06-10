namespace FirstApi.Interfaces;

using FirstApi.Models.DTOs.DoctorSpecialities;

public interface IAuthenticationService
{
    public Task<UserLoginResponse> Login(UserLoginRequest user);
}