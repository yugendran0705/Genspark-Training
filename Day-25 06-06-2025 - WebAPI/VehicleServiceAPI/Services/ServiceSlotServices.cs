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
    public class ServiceSlotService : IServiceSlotService
    {
        private readonly ServiceSlotRepository _serviceSlotRepository;
        private readonly UserRepository _userRepository;

        public ServiceSlotService(ServiceSlotRepository serviceSlotRepository, UserRepository userRepository)
        {
            _serviceSlotRepository = serviceSlotRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Retrieves a service slot by its ID and converts it to a DTO.
        /// </summary>
        public async Task<ServiceSlotDTO> GetServiceSlotByIdAsync(int id)
        {
            var slot = await _serviceSlotRepository.GetByIdAsync(id);
            return await MapServiceSlotToDto(slot);
        }

        /// <summary>
        /// Retrieves all service slots as a collection of DTOs.
        /// </summary>
        public async Task<IEnumerable<ServiceSlotDTO>> GetAllServiceSlotsAsync()
        {
            var slots = await _serviceSlotRepository.GetAllAsync();
            var slotDtos = new List<ServiceSlotDTO>();

            foreach (var slot in slots)
            {
                var dto = await MapServiceSlotToDto(slot);
                slotDtos.Add(dto);
            }

            return slotDtos;
        }

        /// <summary>
        /// Creates a new service slot from the provided creation DTO.
        /// </summary>
        public async Task<ServiceSlotDTO> CreateServiceSlotAsync(CreateServiceSlotDTO request)
        {
            if(request.SlotDateTime < DateTime.UtcNow)
            {
                throw new ArgumentException("Slot date and time cannot be in the past.");
            }
            var slotEntity = await MapCreateDTOToServiceSlot(request);
            var createdSlot = await _serviceSlotRepository.AddAsync(slotEntity);
            return await MapServiceSlotToDto(createdSlot);
        }

        /// <summary>
        /// Updates an existing service slot with the provided update DTO.
        /// </summary>
        public async Task<ServiceSlotDTO> UpdateServiceSlotAsync(int id, UpdateServiceSlotDTO request)
        {
            // Retrieve the existing service slot.
            var slot = await _serviceSlotRepository.GetByIdAsync(id);
            if (slot == null)
            {
                throw new InvalidOperationException("Service slot not found.");
            }

            // Update the properties.
            slot.SlotDateTime = request.SlotDateTime;
            slot.Status = request.Status;
            slot.MechanicID = request.MechanicID;

            var updatedSlot = await _serviceSlotRepository.UpdateAsync(slot);
            return await MapServiceSlotToDto(updatedSlot);
        }

        /// <summary>
        /// Deletes (soft-deletes) a service slot by its ID.
        /// </summary>
        public async Task<bool> DeleteServiceSlotAsync(int id)
        {
            return await _serviceSlotRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Retrieves all available service slots.
        /// </summary>
        public async Task<IEnumerable<ServiceSlotDTO>> GetAvailableSlotsAsync()
        {
            var slots = await _serviceSlotRepository.GetAvailableSlotsAsync();
            var slotDtos = new List<ServiceSlotDTO>();

            foreach (var slot in slots)
            {
                var dto = await MapServiceSlotToDto(slot);
                slotDtos.Add(dto);
            }

            return slotDtos;
        }

        /// <summary>
        /// Retrieves all service slots assigned to a specific mechanic.
        /// </summary>
        public async Task<IEnumerable<ServiceSlotDTO>> GetSlotsByMechanicIdAsync(int mechanicId)
        {
            var slots = await _serviceSlotRepository.GetSlotsByMechanicIdAsync(mechanicId);
            var slotDtos = new List<ServiceSlotDTO>();

            foreach (var slot in slots)
            {
                var dto = await MapServiceSlotToDto(slot);
                slotDtos.Add(dto);
            }

            return slotDtos;
        }

        #region Mapping Methods

        // Maps a ServiceSlot domain model
        private async Task<ServiceSlotDTO> MapServiceSlotToDto(ServiceSlot serviceSlot)
        {
            // Retrieve the mechanic details using the MechanicID from the service slot.
            var mechanic = await _userRepository.GetByIdAsync(serviceSlot.MechanicID);

            return new ServiceSlotDTO
            {
                Id = serviceSlot.Id,
                SlotDateTime = serviceSlot.SlotDateTime,
                Status = serviceSlot.Status,
                MechanicID = serviceSlot.MechanicID,
                MechanicName = mechanic?.Name ?? string.Empty
            };
        }

        private async Task<ServiceSlot> MapCreateDTOToServiceSlot(CreateServiceSlotDTO request)
        {
            // Retrieve the mechanic based on the provided MechanicID.
            var mechanic = await _userRepository.GetByIdAsync(request.MechanicID);

            return new ServiceSlot
            {
                SlotDateTime = request.SlotDateTime,
                Status = request.Status,
                MechanicID = request.MechanicID,
                Mechanic = mechanic
            };
        }

        #endregion
    }
}