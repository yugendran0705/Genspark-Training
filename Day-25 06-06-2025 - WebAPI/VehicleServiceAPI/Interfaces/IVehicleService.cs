using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleServiceAPI.Models.DTOs;

namespace VehicleServiceAPI.Interfaces
{
    public interface IVehicleService
    {
        /// <summary>
        /// Retrieves a vehicle by its ID and maps it to a DTO.
        /// </summary>
        Task<VehicleDTO> GetVehicleByIdAsync(int id);

        /// <summary>
        /// Retrieves all vehicles as a collection of DTOs.
        /// </summary>
        Task<IEnumerable<VehicleDTO>> GetAllVehiclesAsync();

        /// <summary>
        /// Creates a new vehicle from the provided creation request DTO.
        /// </summary>
        Task<VehicleDTO> CreateVehicleAsync(int userId, CreateVehicleDTO request);

        /// <summary>
        /// Updates an existing vehicle with the provided data.
        /// </summary>
        Task<VehicleDTO> UpdateVehicleAsync(int id, int userId, UpdateVehicleDTO request);

        /// <summary>
        /// Deletes (or removes) a vehicle by its ID.
        /// </summary>
        Task<bool> DeleteVehicleAsync(int id, int userId);

        Task<IEnumerable<VehicleDTO>> GetVehicleByUserAsync(int userId);
    }
}
