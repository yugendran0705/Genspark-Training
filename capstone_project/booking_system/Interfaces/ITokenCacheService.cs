namespace BookingSystem.Interfaces;
public interface ITokenCacheService
{
    void StoreToken(string token);
    void RemoveToken(string token);
    bool IsTokenValid(string token);
}
