using System;
using System.Collections.Generic;
using System.Linq;
using CardioAppointments.Interfaces;
using CardioAppointments.Models;

namespace CardioAppointments.Repositories
{
    public class AppointmentRepository : IRepositor<int, Appointment>
    {
        private readonly List<Appointment> _appointments = new List<Appointment>();
        private int _counter = 0;

        public Appointment Add(Appointment item)
        {
            item.Id = ++_counter;
            // (Optional) Check for duplicates based on criteria if needed.
            _appointments.Add(item);
            return item;
        }

        public Appointment Delete(int id)
        {
            Appointment appointment = GetById(id);
            _appointments.Remove(appointment);
            return appointment;
        }

        public ICollection<Appointment> GetAll()
        {
            return _appointments;
        }

        public Appointment GetById(int id)
        {
            Appointment? appointment = _appointments.FirstOrDefault(a => a.Id == id);
            if (appointment == null)
                throw new KeyNotFoundException("Appointment not found with ID: " + id);
            return appointment;
        }

        public Appointment Update(Appointment item)
        {
            Appointment appointment = GetById(item.Id);
            int index = _appointments.IndexOf(appointment);
            _appointments[index] = item;
            return item;
        }
    }
}
