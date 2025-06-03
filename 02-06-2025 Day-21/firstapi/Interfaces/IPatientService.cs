namespace FirstApi.Interfaces;

using FirstApi.Models;
using FirstApi.Models.DTOs.Patient;
public interface IPatientService
{
    public Task<Patient> AddPatient(PatientAddDto patientDto);
    public Task<IEnumerable<Patient>> GetAllPatients();
}