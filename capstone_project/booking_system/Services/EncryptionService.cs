namespace BookingSystem.Services;

using System.Security.Cryptography;
using System.Text;
using BookingSystem.Interfaces;
using BookingSystem.Models;
using BCrypt.Net;
public class EncryptionService : IEncryptionService
{

    public async Task<EncryptModel> EncryptData(EncryptModel data)
    {
        string hashedPassword = BCrypt.HashPassword(data.Data);
        data.EncryptedData = hashedPassword;
        return await Task.FromResult(data);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Verify(password, hashedPassword);
    }

}