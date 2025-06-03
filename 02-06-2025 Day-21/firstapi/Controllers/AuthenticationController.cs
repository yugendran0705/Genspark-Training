
using FirstApi.Interfaces;
using FirstApi.Models.DTOs.DoctorSpecialities;
using Microsoft.AspNetCore.Mvc;
using FirstApi.Models;

using Microsoft.AspNetCore.Authorization;



[ApiController]
[Route("/api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
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
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Unauthorized(e.Message);
        }
    }
}