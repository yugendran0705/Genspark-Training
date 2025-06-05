namespace FirstApi.Models.DTOs.DoctorSpecialities;

public class DoctorsByNameResponseDto
{
    public int Id { get; set; }
    public string Dname { get; set; } = string.Empty;
    public float Yoe { get; set; }
}
