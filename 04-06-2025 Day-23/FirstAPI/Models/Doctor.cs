namespace FirstAPI.Models
{


    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public float YearsOfExperience { get; set; }
        public string Email { get; set; } = string.Empty;
        public ICollection<DoctorSpeciality>? DoctorSpecialities { get; set; }
        public ICollection<Appointmnet>? Appointmnets { get; set; }
         
        public User? User { get; set; }

    }
}