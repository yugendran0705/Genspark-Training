using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthController(IConfiguration config, IHttpClientFactory httpClientFactory)
    {
        _config = config;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("google-login")]
    public IActionResult GoogleLogin()
    {
        //google oauth url is being returned here
        var clientId = _config["Authentication:Google:ClientId"];
        var redirectUri = "http://localhost:5029/api/auth/google-callback";
        var scope = "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile";

        var authUrl = $"https://accounts.google.com/o/oauth2/v2/auth" +
                      $"?response_type=code" +
                      $"&client_id={clientId}" +
                      $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                      $"&scope={Uri.EscapeDataString(scope)}" +
                      $"&access_type=offline" +
                      $"&prompt=consent";

        return Redirect(authUrl);
    }

    //upon account selection, Google redirects to this endpoint with a code(the redirect url that we have given in console along with the code)

    [HttpGet("google-callback")]
    public async Task<IActionResult> GoogleCallback([FromQuery] string code)
    {
        var clientId = _config["Authentication:Google:ClientId"];
        var clientSecret = _config["Authentication:Google:ClientSecret"];
        var redirectUri = "http://localhost:5029/api/auth/google-callback";

        var client = _httpClientFactory.CreateClient();

        //using the code, we will exchange it for an access token
        var tokenResponse = await client.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["code"] = code,
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
            ["redirect_uri"] = redirectUri,
            ["grant_type"] = "authorization_code"
        }));

        if (!tokenResponse.IsSuccessStatusCode)
            return BadRequest("Token exchange failed");

        var tokenData = JsonSerializer.Deserialize<JsonElement>(await tokenResponse.Content.ReadAsStringAsync());
        var accessToken = tokenData.GetProperty("access_token").GetString();

        // Get user info
        var userRequest = new HttpRequestMessage(HttpMethod.Get, "https://www.googleapis.com/oauth2/v2/userinfo");
        userRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var userResponse = await client.SendAsync(userRequest);
        if (!userResponse.IsSuccessStatusCode)
            return BadRequest("Failed to retrieve user info");

        var userJson = await userResponse.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<JsonElement>(userJson);

        var email = user.GetProperty("email").GetString();
        var name = user.GetProperty("name").GetString();
        Console.WriteLine($"User Email: {email}, Name: {name}");

        //we can use our own token or jwt generation logic and use here

        return Ok(new
        {
            Email = email,
            Name = name,
            Message = "User authenticated using Google"
        });
    }

   
}
