using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleServiceAPI.DTOs;

namespace VehicleServiceAPI.Interfaces
{
    public interface IServiceSlotService
    {
        Task<IEnumerable<ServiceSlotDTO>> GetAllServiceSlotsAsync();
    }
}
