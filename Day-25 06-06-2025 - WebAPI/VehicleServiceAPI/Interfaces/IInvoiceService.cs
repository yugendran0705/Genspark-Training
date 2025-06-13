using System.Threading.Tasks;
using VehicleServiceAPI.Models.DTOs;

namespace VehicleServiceAPI.Interfaces
{
    public interface IInvoiceService
    {
        Task<InvoiceDTO> GetInvoiceByIdAsync(int id);
        Task<IEnumerable<InvoiceDTO>> GetAllInvoicesAsync();
        Task<InvoiceDTO> CreateInvoiceAsync(CreateInvoiceDTO request);
        Task<InvoiceDTO> UpdateInvoiceAsync(UpdateInvoiceDTO request);
        Task<IEnumerable<InvoiceDTO>> GetInvoicesByBookingIdAsync(int bookingId);
        Task<bool> DeleteInvoiceAsync(int id);
    }
}
