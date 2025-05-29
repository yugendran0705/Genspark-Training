namespace FirstAPI.Models.DTOs.DoctorSpecialities
{
    public class DoctorAddRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<SpecialityAddRequestDto> Specialities { get; set; } = new List<SpecialityAddRequestDto>();
        public float YearsOfExperience { get; set; }
    }
}
