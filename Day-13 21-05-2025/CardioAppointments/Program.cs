using System;
using CardioAppointments.Models;
using CardioAppointments.Repositories;
using CardioAppointments.Services;
using CardioAppointments.Interfaces;

namespace CardioAppointments
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Setup the repository and service.
            IRepositor<int, Appointment> repository = new AppointmentRepository();
            IAppointmentService appointmentService = new AppointmentService(repository);

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n--- Cardiologist Appointment Management ---");
                Console.WriteLine("1. Add New Appointment");
                Console.WriteLine("2. Search Appointments");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");
                string option = Console.ReadLine() ?? "";

                switch (option)
                {
                    case "1":
                        // Add new appointment.
                        Appointment newAppointment = new Appointment();
                        Console.Write("Enter Patient Name: ");
                        newAppointment.PatientName = Console.ReadLine() ?? "";

                        Console.Write("Enter Patient Age: ");
                        int age;
                        while (!int.TryParse(Console.ReadLine(), out age))
                        {
                            Console.WriteLine("Please enter a valid age.");
                        }
                        newAppointment.PatientAge = age;

                        Console.Write("Enter Appointment Date and Time (yyyy-MM-dd HH:mm): ");
                        DateTime apptDate;
                        while (!DateTime.TryParse(Console.ReadLine(), out apptDate))
                        {
                            Console.WriteLine("Please enter a valid date and time.");
                        }
                        newAppointment.AppointmentDate = apptDate;

                        Console.Write("Enter Reason for Visit: ");
                        newAppointment.Reason = Console.ReadLine() ?? "";

                        int id = appointmentService.AddAppointment(newAppointment);
                        if (id != -1)
                        {
                            Console.WriteLine("Appointment added successfully with ID: " + id);
                        }
                        else
                        {
                            Console.WriteLine("Failed to add appointment.");
                        }
                        break;

                    case "2":
                        // Search for appointments.
                        AppointmentSearchModel searchModel = new AppointmentSearchModel();

                        Console.Write("Enter Patient Name to search (optional): ");
                        string? searchName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(searchName))
                        {
                            searchModel.PatientName = searchName;
                        }

                        Console.Write("Enter Appointment Date to search (yyyy-MM-dd, optional): ");
                        string? dateInput = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(dateInput))
                        {
                            if (DateTime.TryParse(dateInput, out DateTime searchDate))
                            {
                                searchModel.AppointmentDate = searchDate;
                            }
                        }

                        Console.Write("Enter minimum Patient Age (optional): ");
                        string? minAgeInput = Console.ReadLine();
                        Console.Write("Enter maximum Patient Age (optional): ");
                        string? maxAgeInput = Console.ReadLine();
                        if (int.TryParse(minAgeInput, out int minAge) && int.TryParse(maxAgeInput, out int maxAge))
                        {
                            searchModel.AgeRange = new Range<int> { MinVal = minAge, MaxVal = maxAge };
                        }

                        var results = appointmentService.SearchAppointments(searchModel);
                        if (results != null && results.Count > 0)
                        {
                            Console.WriteLine("\nMatching Appointments:");
                            foreach (var app in results)
                            {
                                Console.WriteLine($"ID: {app.Id} | Name: {app.PatientName} | Age: {app.PatientAge} | Date: {app.AppointmentDate} | Reason: {app.Reason}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No appointments found for the given criteria.");
                        }
                        break;

                    case "3":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }

            Console.WriteLine("Exiting application. Goodbye!");
        }
    }
}
