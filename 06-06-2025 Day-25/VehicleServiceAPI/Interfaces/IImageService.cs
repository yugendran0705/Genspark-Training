using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VehicleServiceAPI.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
