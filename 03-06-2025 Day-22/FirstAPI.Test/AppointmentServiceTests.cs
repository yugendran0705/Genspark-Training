using System;
using System.Threading.Tasks;
using FirstAPI.Contexts;
using FirstAPI.Models;
using FirstAPI.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FirstAPI.Test
{
    [TestFixture]
    public class AppointmentServiceTests
    {
        private ClinicContext _context;

        [SetUp]
        public void Setup()
        {
            // Use a unique in-memory database name for each test to prevent data collisions.
            var options = new DbContextOptionsBuilder<ClinicContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ClinicContext(options);
        }

        [Test]
        public async Task CancelAppointmentTest_DoctorExperienceRestrictions()
        {
            // Arrange

            // Create a doctor with insufficient experience (< 3 years)
            var doctorLow = new Doctor
            {
                Name = "Dr. LowExp",
                YearsOfExperience = 2,
                Email = "lowexp@gmail.com"
            };
            _context.Add(doctorLow);
            await _context.SaveChangesAsync();

            // Create an Appointment for the low-experience doctor
            var appointmentLow = new Appointment
            {
                AppointmentNumber = "A1",
                DoctorId = doctorLow.Id,
                AppointmentDateTime = DateTime.Now.AddDays(1),
                Status = "Scheduled"
            };
            _context.Add(appointmentLow);
            await _context.SaveChangesAsync();

            // Create a doctor with sufficient experience (>= 3 years)
            var doctorHigh = new Doctor
            {
                Name = "Dr. HighExp",
                YearsOfExperience = 5,
                Email = "highexp@gmail.com"
            };
            _context.Add(doctorHigh);
            await _context.SaveChangesAsync();

            // Create an Appointment for the high-experience doctor
            var appointmentHigh = new Appointment
            {
                AppointmentNumber = "A2",
                DoctorId = doctorHigh.Id,
                AppointmentDateTime = DateTime.Now.AddDays(1),
                Status = "Scheduled"
            };
            _context.Add(appointmentHigh);
            await _context.SaveChangesAsync();

            // Instantiate the AppointmentService from the services folder
            var appointmentService = new AppointmentService(_context);

            // Act & Assert

            // Test that cancellation fails for the less experienced doctor.
            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await appointmentService.CancelAppointment(appointmentLow.AppointmentNumber, doctorLow.Id));
            Assert.That(ex.Message, Is.EqualTo("Only doctors with at least 3 years of experience can cancel an Appointment"));

            // Test that cancellation succeeds for the experienced doctor.
            Assert.DoesNotThrowAsync(async () =>
                await appointmentService.CancelAppointment(appointmentHigh.AppointmentNumber, doctorHigh.Id));

            // Verify the Appointment for the experienced doctor is marked as cancelled.
            var updatedAppointment = await _context.Appointments.FindAsync(appointmentHigh.AppointmentNumber);
            Assert.That(updatedAppointment.Status, Is.EqualTo("Cancelled"));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
