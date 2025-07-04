using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models;
using VehicleServiceAPI.Models.DTOs;
using VehicleServiceAPI.Repositories;

namespace VehicleServiceAPI.Services
{
    public class BookingService : IBookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly UserRepository _userRepository;
        private readonly ServiceSlotRepository _serviceSlotRepository;
        private readonly VehicleRepository _vehicleRepository;

        public BookingService(BookingRepository bookingRepository, UserRepository userRepository, ServiceSlotRepository serviceSlotRepository, VehicleRepository vehicleRepository)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _serviceSlotRepository = serviceSlotRepository;
            _vehicleRepository = vehicleRepository;
        }

        /// <summary>
        /// Retrieves a booking by its ID and maps it to a BookingDTO.
        /// </summary>
        public async Task<BookingDTO> GetBookingByIdAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            return await MapBookingToDto(booking);
        }

        /// <summary>
        /// Retrieves all bookings as a collection of BookingDTOs.
        /// </summary>
        public async Task<IEnumerable<BookingDTO>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            var bookingDtos = new List<BookingDTO>();

            foreach (var booking in bookings)
            {
                var dto = await MapBookingToDto(booking);
                bookingDtos.Add(dto);
            }

            return bookingDtos;
        }

        /// <summary>
        /// Creates a new booking for a given user based on the provided CreateBookingDTO.
        /// </summary>
        public async Task<BookingDTO> CreateBookingAsync(int userId, CreateBookingDTO request)
        {
            // Map the incoming DTO to a Booking entity.
            var bookingEntity = await MapCreateBookingDtoToBooking(userId, request);
            if (bookingEntity.ServiceSlot.Status == "booked")
            {
                throw new InvalidOperationException("The selected service slot is already booked.");
            }
            // Mark the selected slot as booked.
            bookingEntity.ServiceSlot.Status = "booked";
            await _serviceSlotRepository.UpdateAsync(bookingEntity.ServiceSlot);

            var createdBooking = await _bookingRepository.AddAsync(bookingEntity);
            return await MapBookingToDto(createdBooking);
        }

        /// <summary>
        /// Updates an existing booking with the provided update DTO.
        /// Also validates that the booking belongs to the given user.
        /// </summary>
        public async Task<BookingDTO> UpdateBookingAsync(int id, UpdateBookingDTO request)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            booking.Status = request.Status;

            var updatedBooking = await _bookingRepository.UpdateAsync(booking);
            return await MapBookingToDto(updatedBooking);
        }

        /// <summary>
        /// Soft-deletes a booking by its ID.
        /// </summary>
        public async Task<bool> DeleteBookingAsync(int id)
        {
            return await _bookingRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Retrieves all bookings made by a specific user.
        /// </summary>
        public async Task<IEnumerable<BookingDTO>> GetBookingsByUserIdAsync(int userId)
        {
            var bookings = await _bookingRepository.GetBookingsByUserIdAsync(userId);
            var bookingDtos = new List<BookingDTO>();

            foreach (var booking in bookings)
            {
                var dto = await MapBookingToDto(booking);
                bookingDtos.Add(dto);
            }

            return bookingDtos;
        }

        /// <summary>
        /// Retrieves all bookings filtered by a given status.
        public async Task<IEnumerable<BookingDTO>> GetBookingsByStatusAsync(string status)
        {
            var bookings = await _bookingRepository.GetBookingsByStatusAsync(status);
            var bookingDtos = new List<BookingDTO>();

            foreach (var booking in bookings)
            {
                var dto = await MapBookingToDto(booking);
                bookingDtos.Add(dto);
            }

            return bookingDtos;
        }

        #region Mapping Methods

        // Maps a Booking domain model to a BookingDTO.
        private async Task<BookingDTO> MapBookingToDto(Booking booking)
        {
            // Console.WriteLine($"Id: {booking.Id}, UserId: {booking.UserId}, Name: {booking.User.Name}, Email: {booking.User.Email}, Phone: {booking.User.Phone}, SlotId: {booking.SlotId}, SlotStatus: {booking.ServiceSlot.Status}, SlotDateTime: {booking.ServiceSlot.SlotDateTime}, MechanicID: {booking.ServiceSlot.MechanicID}, MechanicName: {booking.ServiceSlot.Mechanic.Name}, VehicleId: {booking.VehicleId}, Make: {booking.Vehicle.Make}, Model: {booking.Vehicle.Model}, Year: {booking.Vehicle.Year}, RegistrationNumber: {booking.Vehicle.RegistrationNumber}, Status: {booking.Status}");
            var mechanic = await _userRepository.GetByIdAsync(booking.ServiceSlot.MechanicID);
            
            return new BookingDTO
            {
                Id = booking.Id,
                UserId = booking.UserId,
                Name = booking.User.Name,
                Email = booking.User.Email,
                Phone = booking.User.Phone,
                SlotId = booking.SlotId,
                SlotStatus = booking.ServiceSlot.Status,
                SlotDateTime = booking.ServiceSlot.SlotDateTime,
                MechanicID = booking.ServiceSlot.MechanicID,
                MechanicName = mechanic.Name,
                VehicleId = booking.VehicleId,
                Make = booking.Vehicle.Make,
                Model = booking.Vehicle.Model,
                Year = booking.Vehicle.Year,
                RegistrationNumber = booking.Vehicle.RegistrationNumber,
                Status = booking.Status,
            };
        }

        // Maps a CreateBookingDTO and userId to a Booking domain model.
        private async Task<Booking> MapCreateBookingDtoToBooking(int userId, CreateBookingDTO request)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            Console.WriteLine("SlotID",request.SlotId);
            var slot = await _serviceSlotRepository.GetByIdAsync(request.SlotId);
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);
            return new Booking
            {
                // Use a default status if not provided, for example "pending".
                Status = "pending",
                SlotId = request.SlotId,
                ServiceSlot = slot,
                VehicleId = request.VehicleId,
                Vehicle = vehicle,
                UserId = userId,
                User = user
            };
        }

        #endregion
    }
}
