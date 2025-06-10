using System.Threading.Tasks;
using VehicleServiceAPI.DTOs;

namespace VehicleServiceAPI.Interfaces
{
    public interface IVehicleService
    {
        Task<VehicleDTO> CreateVehicleAsync(VehicleDTO.CreateVehicleDTO vehicleDto);
    }
}
