using System.Threading.Tasks;
using VehicleServiceAPI.DTOs;

namespace VehicleServiceAPI.Interfaces
{
    public interface IBookingService
    {
        Task<BookingDTO> CreateBookingAsync(BookingDTO.CreateBookingDTO bookingDto);
        Task<BookingDTO> GetBookingByIdAsync(int bookingId);
        Task<BookingDTO> UpdateBookingAsync(int bookingId, BookingDTO bookingDto);
        Task<bool> DeleteBookingAsync(int bookingId);
        Task<IEnumerable<BookingDTO>> GetBookingsByUserIdAsync(int userId);
        Task<IEnumerable<BookingDTO>> GetAllBookingsAsync();
    }
}
