namespace BookingSystem.Interfaces;
using BookingSystem.Models;
public interface IEncryptionService
{
    public Task<EncryptModel> EncryptData(EncryptModel data);
    public bool VerifyPassword(string password, string hashedPassword);
}