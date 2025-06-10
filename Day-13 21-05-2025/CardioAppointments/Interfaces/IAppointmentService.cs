using System.Collections.Generic;
using CardioAppointments.Models;

namespace CardioAppointments.Interfaces
{
    public interface IAppointmentService
    {
        int AddAppointment(Appointment appointment);
        List<Appointment>? SearchAppointments(AppointmentSearchModel searchModel);
    }
}
