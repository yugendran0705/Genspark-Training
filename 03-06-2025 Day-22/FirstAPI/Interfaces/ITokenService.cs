namespace FirstApi.Interfaces;
using FirstApi.Models;
public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
    }