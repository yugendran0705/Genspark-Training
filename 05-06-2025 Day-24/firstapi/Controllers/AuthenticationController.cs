
using FirstApi.Interfaces;
using FirstApi.Models.DTOs.DoctorSpecialities;
using Microsoft.AspNetCore.Mvc;
using FirstApi.Models;
using FirstApi.Misc;

using Microsoft.AspNetCore.Authorization;
using AspNet.Security.OAuth.GitHub;
using Microsoft.AspNetCore.Authentication;


[ApiController]
[Route("/api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly FirstApi.Interfaces.IAuthenticationService _authenticationService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(FirstApi.Interfaces.IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }
    [HttpPost]
    [CustomExceptionFilter]
    public async Task<ActionResult<UserLoginResponse>> UserLogin(UserLoginRequest loginRequest)
    {

        var result = await _authenticationService.Login(loginRequest);
        return Ok(result);

    }

    
    
}