namespace DocumentSharingAPI.Models.Dtos
{
    public class UserRegisterDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        // Optionally, you can enforce a default role
        public string Role { get; set; }
    }
}
