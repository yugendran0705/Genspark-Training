namespace BookingSystem.Interfaces;
using BookingSystem.Models;
using BookingSystem.Models.DTOs ;

public interface IAdminService
{
    Task<Admin> RegisterAdmin(AdminDto adminDto);
    Task<Admin> GetAdminByName(string name);
    Task<IEnumerable<Admin>> GetAllAdmins();
    Task<Admin> UpdateAdmin(AdminDto adminDto);
}
