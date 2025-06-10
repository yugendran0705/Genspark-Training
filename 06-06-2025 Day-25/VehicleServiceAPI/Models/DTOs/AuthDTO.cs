namespace VehicleServiceAPI.DTOs
{
    public class AuthDTO
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

        public class AuthResponseDTO
        {
            public string Token { get; set; }
            public string RefreshToken { get; set; }
            public int UserId { get; set; }
        }
        
        public class RefreshTokenDTO
        {
            public string Token { get; set; }
            public string RefreshToken { get; set; }
        }
    }
}
