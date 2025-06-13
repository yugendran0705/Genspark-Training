using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleServiceAPI.Models.DTOs;

namespace VehicleServiceAPI.Interfaces
{
    public interface IServiceSlotService
    {
        Task<ServiceSlotDTO> GetServiceSlotByIdAsync(int id);
        Task<IEnumerable<ServiceSlotDTO>> GetAllServiceSlotsAsync();
        Task<ServiceSlotDTO> CreateServiceSlotAsync(CreateServiceSlotDTO request);
        Task<ServiceSlotDTO> UpdateServiceSlotAsync(int id, UpdateServiceSlotDTO request);
        Task<bool> DeleteServiceSlotAsync(int id);
        Task<IEnumerable<ServiceSlotDTO>> GetAvailableSlotsAsync();
        Task<IEnumerable<ServiceSlotDTO>> GetSlotsByMechanicIdAsync(int mechanicId);
    }
}
