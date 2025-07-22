

namespace BookingSystem.Services;

using BookingSystem.Interfaces;
using BookingSystem.Models;
using BookingSystem.Models.DTOs;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

public class AuthenticationService : IAuthenticationService
{
    private readonly ITokenService _tokenService;
    private readonly IEncryptionService _encryptionService;
    private readonly IRepository<string, User> _userRepository;
    private readonly ITokenCacheService _tokenCacheService;
    private readonly ILogger<AuthenticationService> _logger;

    private static readonly ConcurrentDictionary<string, string> refreshTokens = new();

    public AuthenticationService(ITokenService tokenService,
                                IEncryptionService encryptionService,
                                IRepository<string, User> userRepository,
                                ILogger<AuthenticationService> logger,
                                ITokenCacheService tokenCacheService)
    {
        _tokenService = tokenService;
        _encryptionService = encryptionService;
        _userRepository = userRepository;
        _logger = logger;
        _tokenCacheService = tokenCacheService;
    }

    public async Task<UserLoginResponse> Login(UserLoginRequest user)
    {
        var dbUser = await _userRepository.Get(user.Username);
        if (dbUser == null)
        {
            _logger.LogCritical("User not found");
            throw new Exception("No such user");
        }

    
        var encryptedData = _encryptionService.VerifyPassword(user.Password,dbUser.Password);

        if (encryptedData==false)
        {
            _logger.LogError("Invalid login attempt");
            throw new Exception("Invalid password");
        }

        var token = await _tokenService.GenerateToken(dbUser);
        //Console.WriteLine($"Token for {dbUser.Email}: {token}");
        var refreshToken = Guid.NewGuid().ToString();

        // store refresh token
        refreshTokens[dbUser.Email] = refreshToken;

        return new UserLoginResponse
        {
            Username = user.Username,
            Role=dbUser.Role,
            Token = token,
            RefreshToken = refreshToken
        };
    }

    public async Task Logout(string email,string token)
    {
        // remove refresh token from store
        if (refreshTokens.ContainsKey(email))
        {
            refreshTokens.TryRemove(email, out _);
            _tokenCacheService.RemoveToken(token);
            _logger.LogInformation($"User {email} logged out successfully.");
        }
        else
        {
            _logger.LogWarning($"Logout attempted for unknown user {email}.");
        }

        await Task.CompletedTask;
    }

    public async Task<UserLoginResponse> RefreshToken(string email, string refreshToken)
    {
        if (!refreshTokens.TryGetValue(email, out var storedToken) || storedToken != refreshToken)
        {
            _logger.LogError("Invalid refresh token");
            throw new Exception("Invalid refresh token");
        }

        var dbUser = await _userRepository.Get(email);
        if (dbUser == null)
        {
            throw new Exception("User not found");
        }

        var newToken = await _tokenService.GenerateToken(dbUser);
        var newRefreshToken = Guid.NewGuid().ToString();    

        refreshTokens[email] = newRefreshToken;

        return new UserLoginResponse
        {
            Username = dbUser.Email,
            Token = newToken,
            RefreshToken = newRefreshToken
        };
    }
}
