namespace DocumentSharingAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        // For production, store a salted hash instead of plain text.
        public string PasswordHash { get; set; }
        public string Role { get; set; } // Example: "HRAdmin" or "User"
    }
}
