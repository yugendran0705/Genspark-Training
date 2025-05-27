using System;
using System.Collections.Generic;
using System.Linq;
using CardioAppointments.Interfaces;
using CardioAppointments.Models;

namespace CardioAppointments.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepositor<int, Appointment> _appointmentRepository;

        public AppointmentService(IRepositor<int, Appointment> appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        // Adds a new appointment; returns the appointment ID if successful.
        public int AddAppointment(Appointment appointment)
        {
            try
            {
                var addedAppointment = _appointmentRepository.Add(appointment);
                if (addedAppointment != null)
                    return addedAppointment.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error adding appointment: " + e.Message);
            }
            return -1;
        }

        // Searches appointments based on the search model filters.
        public List<Appointment>? SearchAppointments(AppointmentSearchModel searchModel)
        {
            try
            {
                var appointments = _appointmentRepository.GetAll();
                appointments = SearchByName(appointments, searchModel.PatientName);
                appointments = SearchByDate(appointments, searchModel.AppointmentDate);
                appointments = SearchByAge(appointments, searchModel.AgeRange);

                if (appointments != null && appointments.Count > 0)
                    return appointments.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error searching appointments: " + e.Message);
            }
            return null;
        }

        // Filter by patient name (case-insensitive partial match).
        private ICollection<Appointment> SearchByName(ICollection<Appointment> appointments, string? name)
        {
            if (string.IsNullOrEmpty(name) || appointments == null || appointments.Count == 0)
                return appointments ?? [];

            return appointments.Where(a => a.PatientName.ToLower().Contains(name.ToLower())).ToList();
        }

        // Filter by appointment date (exact match on the date portion).
        private ICollection<Appointment> SearchByDate(ICollection<Appointment> appointments, DateTime? appointmentDate)
        {
            if (appointmentDate == null || appointments == null || appointments.Count == 0)
                return appointments ?? [];

            return appointments.Where(a => a.AppointmentDate.Date == appointmentDate.Value.Date).ToList();
        }

        // Filter appointments by the provided patient age range.
        private ICollection<Appointment> SearchByAge(ICollection<Appointment> appointments, Range<int>? ageRange)
        {
            if (ageRange == null || appointments == null || appointments.Count == 0)
                return appointments ?? [];

            return [.. appointments.Where(a =>
                a.PatientAge >= ageRange.MinVal && a.PatientAge <= ageRange.MaxVal
            )];
        }

        // Updates an existing appointment; returns the updated appointment.
        public Appointment? UpdateAppointment(Appointment appointment)
        {
            try
            {
                var updatedAppointment = _appointmentRepository.Update(appointment);
                if (updatedAppointment != null)
                    return updatedAppointment;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error updating appointment: " + e.Message);
            }
            return null;
        }

        // Deletes an appointment by ID; returns true if successful.
        public bool DeleteAppointment(int appointmentId)
        {
            try
            {
                return _appointmentRepository.Delete(appointmentId) != null;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error deleting appointment: " + e.Message);
            }
            return false;
        }

        // Retrieves all appointments.
        public List<Appointment>? GetAllAppointments()
        {
            try
            {
                var appointments = _appointmentRepository.GetAll();
                if (appointments != null && appointments.Count > 0)
                    return appointments.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error retrieving appointments: " + e.Message);
            }
            return null;
        }
    }
}
