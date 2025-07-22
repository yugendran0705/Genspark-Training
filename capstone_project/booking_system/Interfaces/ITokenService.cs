namespace BookingSystem.Interfaces;

using BookingSystem.Models;
public interface ITokenService
{
    public Task<string> GenerateToken(User user);
}