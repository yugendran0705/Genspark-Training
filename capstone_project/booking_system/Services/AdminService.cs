namespace BookingSystem.Services;

using BookingSystem.Models;
using BookingSystem.Repositories;
using BookingSystem.Interfaces;
using BookingSystem.Models.DTOs;

public class AdminService : IAdminService
{
    private readonly IRepository<string, Admin> _adminRepository;
    private readonly IRepository<string, User> _userRepository;

    private readonly IEncryptionService _encryptionService;

    public AdminService(IRepository<string, Admin> adminRepository, IRepository<string, User> userRepository, IEncryptionService encryptionService)
    {
        _adminRepository = adminRepository;
        _userRepository = userRepository;
        _encryptionService = encryptionService;
    }

    public async Task<Admin> RegisterAdmin(AdminDto adminDto)
    {
        
            var existingUser = await _userRepository.Get(adminDto.Email);
            if (existingUser != null)
        {
            throw new Exception("A user with this email already exists.");
        }
            var encrypteddata = await _encryptionService.EncryptData(new EncryptModel
            {
                Data = adminDto.Password
            });
            var user = new User()
            {
                Name = adminDto.Name,
                Email = adminDto.Email,
                Password = encrypteddata.EncryptedData??"",
                Role = "Admin"
            };

            user = await _userRepository.Add(user);

            Wallet wallet = new Wallet
            {
                CustomerEmail = adminDto.Email,
                balance = 0,
                LastUpdated = DateTime.UtcNow
            };

            var admin = new Admin()
            {
                Name = adminDto.Name,
                Email = adminDto.Email,
                PhoneNumber = adminDto.PhoneNumber,
            };
            //add to user before admin
            var newadmin= await _adminRepository.Add(admin);
        

            return newadmin;
        
    }

        public async Task<Admin> UpdateAdmin(AdminDto adminDto)
{
    
    var existingadmin = await _adminRepository.Get(adminDto.Email);
    if (existingadmin == null)
    {
        throw new Exception("admin not found.");
    }

    
    existingadmin.Name = adminDto.Name;
    existingadmin.PhoneNumber = adminDto.PhoneNumber;

  
    await _adminRepository.Update(adminDto.Email, existingadmin);

    return existingadmin;
}

    public async Task<Admin> GetAdminByName(string name)
    {
        return await _adminRepository.Get(name);
    }

    public async Task<IEnumerable<Admin>> GetAllAdmins()
    {
        return await _adminRepository.GetAll();
    }
}
