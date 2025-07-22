
using BookingSystem.Interfaces;
using BookingSystem.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using BookingSystem.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;


[ApiController]
[Route("/api/v1/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly BookingSystem.Interfaces.IAuthenticationService _authenticationService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(BookingSystem.Interfaces.IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }
    [HttpPost]
    public async Task<ActionResult<UserLoginResponse>> UserLogin(UserLoginRequest loginRequest)
    {

        try
        {
            var result = await _authenticationService.Login(loginRequest);
            _logger.LogInformation("User {Username} logged in successfully", loginRequest.Username);

            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Login failed for user {Username}", loginRequest.Username);

            return Unauthorized(e.Message);
        }

    }
    

    [HttpPost("refresh")]
    [Authorize]
    public async Task<ActionResult<UserLoginResponse>> RefreshToken(string email, string refreshToken)
    {
        try
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("User not authenticated");
            }
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Refresh token is required");
            }
            var result = await _authenticationService.RefreshToken(email, refreshToken);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Refresh token failed for user {Email}", email);
            return Unauthorized(e.Message);
        }
    }



}