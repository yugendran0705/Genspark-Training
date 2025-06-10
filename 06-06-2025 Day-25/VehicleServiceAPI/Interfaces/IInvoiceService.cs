using System.Threading.Tasks;
using VehicleServiceAPI.DTOs;

namespace VehicleServiceAPI.Interfaces
{
    public interface IInvoiceService
    {
        Task<InvoiceDTO> GetInvoiceByIdAsync(int id);
    }
}
