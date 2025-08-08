namespace VehicleServiceAPI.Models.DTOs
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }

    public class RoleRequest
    {
        public string Role { get; set; } = string.Empty;
    }
}