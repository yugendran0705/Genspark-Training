namespace FirstApi.Models;
public class Appointment
    {
        public int Id { get; set; }
        public string AppointmentNumber { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDateTime { get; set; }

        public string Status { get; set; } = string.Empty;
        public Doctor? Doctor { get; set; }
        public Patient? Patient { get; set; }
    }