using System.Threading.Tasks;
using VehicleServiceAPI.Models.DTOs;

namespace VehicleServiceAPI.Interfaces
{
    public interface IBookingService
    {
        Task<BookingDTO> CreateBookingAsync(BookingDTO bookingDto);
        Task<BookingDTO> GetBookingByIdAsync(int bookingId);
        Task<BookingDTO> UpdateBookingAsync(int bookingId, BookingDTO bookingDto);
        Task<bool> DeleteBookingAsync(int bookingId);
        Task<IEnumerable<BookingDTO>> GetBookingsByUserIdAsync(int userId);
        Task<IEnumerable<BookingDTO>> GetAllBookingsAsync();
    }
}
