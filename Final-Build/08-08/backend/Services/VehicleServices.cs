using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models;
using VehicleServiceAPI.Models.DTOs;
using VehicleServiceAPI.Repositories;

namespace VehicleServiceAPI.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly VehicleRepository _vehicleRepository;
        private readonly UserRepository _userRepository;

        public VehicleService(VehicleRepository vehicleRepository, UserRepository userRepository)
        {
            _vehicleRepository = vehicleRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Retrieves a vehicle by its ID and converts it to a DTO.
        /// </summary>
        public async Task<VehicleDTO> GetVehicleByIdAsync(int id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            return await MapVehicleToDto(vehicle);
        }

        public async Task<IEnumerable<VehicleDTO>> GetVehicleByUserAsync(int userId)
        {
            var vehicles = await _vehicleRepository.GetAllByUserAsync(userId);
            var vehicleDtos = new List<VehicleDTO>();

            foreach (var vehicle in vehicles)
            {
                var dto = await MapVehicleToDto(vehicle);
                vehicleDtos.Add(dto);
            }

            return vehicleDtos;
        }

        /// <summary>
        /// Retrieves all vehicles as a collection of DTOs.
        /// </summary>
        public async Task<IEnumerable<VehicleDTO>> GetAllVehiclesAsync()
        {
            var vehicles = await _vehicleRepository.GetAllAsync();
            var vehicleDtos = new List<VehicleDTO>();

            foreach (var vehicle in vehicles)
            {
                vehicleDtos.Add(await MapVehicleToDto(vehicle));
            }

            return vehicleDtos;
        }

        /// <summary>
        /// Creates a new vehicle from the provided creation request DTO.
        /// </summary>
        public async Task<VehicleDTO> CreateVehicleAsync(int userId, CreateVehicleDTO request)
        {
            var vehicleEntity = await MapVehicleCreationRequestDtoToVehicle(userId, request);
            var createdVehicle = await _vehicleRepository.AddAsync(vehicleEntity);
            return await MapVehicleToDto(createdVehicle);
        }

        /// <summary>
        /// Updates an existing vehicle with the provided data.
        /// </summary>
        public async Task<VehicleDTO> UpdateVehicleAsync(int id, int userId, UpdateVehicleDTO vehicleDto)
        {
            // Retrieve the existing vehicle.
            var existingVehicle = await _vehicleRepository.GetByIdAsync(id);
            if (existingVehicle.OwnerId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this vehicle.");
            }
            // Update the desired properties.
            existingVehicle.Make = vehicleDto.Make;
            existingVehicle.Model = vehicleDto.Model;
            existingVehicle.Year = vehicleDto.Year;
            existingVehicle.RegistrationNumber = vehicleDto.RegistrationNumber;

            var updatedVehicle = await _vehicleRepository.UpdateAsync(existingVehicle);
            return await MapVehicleToDto(updatedVehicle);
        }

        /// <summary>
        /// Deletes (or removes) a vehicle by its ID.
        /// </summary>
        public async Task<bool> DeleteVehicleAsync(int id, int userId)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle.OwnerId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this vehicle.");
            }
            return await _vehicleRepository.DeleteAsync(id);
        }

        #region Helper Mapping Methods

        // Converts a Vehicle domain model to a VehicleDTO.
        private async Task<VehicleDTO> MapVehicleToDto(Vehicle vehicle)
        {
            var owner = await _userRepository.GetByIdAsync(vehicle.OwnerId);
            return new VehicleDTO
            {
                Id = vehicle.Id,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                RegistrationNumber = vehicle.RegistrationNumber,
                OwnerId = vehicle.OwnerId,
                OwnerName = owner.Name,
                OwnerEmail = owner.Email,
                OwnerPhone = owner.Phone
            };
        }

        // Converts a VehicleCreationRequestDTO to a Vehicle domain model.
        private async Task<Vehicle> MapVehicleCreationRequestDtoToVehicle(int userId, CreateVehicleDTO request)
        {
            var owner = await _userRepository.GetByIdAsync(userId);
            return new Vehicle
            {
                Make = request.Make,
                Model = request.Model,
                Year = request.Year,
                OwnerId = userId,
                Owner = owner,
                RegistrationNumber = request.RegistrationNumber
            };
        }

        #endregion
    }
}
