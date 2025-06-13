using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VehicleServiceAPI.Models.DTOs;

namespace VehicleServiceAPI.Interfaces
{
    public interface IImageService
    {
        Task<ImageDTO> GetImageByIdAsync(int id);
        Task<IEnumerable<ImageDTO>> GetAllImagesAsync();
        Task<ImageDTO> CreateImageAsync(CreateImageDTO request);
        Task<ImageDTO> UpdateImageAsync(int id, UpdateImageDTO request);
        Task<bool> DeleteImageAsync(int id);
        Task<IEnumerable<ImageDTO>> GetImagesByBookingIdAsync(int bookingId);
        Task<IEnumerable<ImageDTO>> GetImagesByVehicleIdAsync(int vehicleId);
    }
}
