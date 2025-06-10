namespace FirstApi.Models.DTOs
{
    public class DoctorAddRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<int>? SpecialityIds { get; set; }
        public float YearsOfExperience { get; set; }
    }
}
