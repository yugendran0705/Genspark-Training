namespace VehicleServiceAPI.Models.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsDeleted { get; set; }
        // public DateTime CreatedAt { get; set; }
    }
    public class UserCreationRequestDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }

    public class UserUpdateRequestDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class UpdateUserRequestDTO
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
