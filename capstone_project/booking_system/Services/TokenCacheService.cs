namespace BookingSystem.Services;
using BookingSystem.Interfaces;
public class InMemoryTokenCacheService : ITokenCacheService
{
    private readonly HashSet<string> _activeTokens = new();

    public void StoreToken(string token)
    {
        _activeTokens.Add(token);
    }

    public void RemoveToken(string token)
    {
        _activeTokens.Remove(token);
    }

    public bool IsTokenValid(string token)
    {
        return _activeTokens.Contains(token);
    }
}
