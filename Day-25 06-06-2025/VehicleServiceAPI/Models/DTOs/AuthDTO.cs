namespace VehicleServiceAPI.Models.DTOs
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }

    public class LoginResponseDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenRequestDTO
    {
        public string RefreshToken { get; set; }        
    }
        
}
