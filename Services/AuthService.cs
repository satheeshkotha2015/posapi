using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PosApi.DTOs;

namespace PosApi.Services;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    // Hardcoded user for demo
    private const string DemoUsername = "admin";
    private const string DemoPassword = "password123";

    public AuthService(IConfiguration configuration, ILogger<AuthService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        _logger.LogInformation("Login attempt for user: {Username}", request.Username);

        // Validate credentials
        if (request.Username != DemoUsername || request.Password != DemoPassword)
        {
            _logger.LogWarning("Invalid credentials for user: {Username}", request.Username);
            return new LoginResponseDto
            {
                Success = false,
                Message = "Invalid credentials"
            };
        }

        // Generate JWT token
        var token = GenerateJwtToken(request.Username);

        _logger.LogInformation("Login successful for user: {Username}", request.Username);

        return await Task.FromResult(new LoginResponseDto
        {
            Success = true,
            Token = token,
            Message = "Login successful"
        });
    }

    private string GenerateJwtToken(string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"] ?? "secret-key-for-pos-system-demo"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, username),
            new Claim(ClaimTypes.Name, username),
            new Claim("role", "admin")
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "pos-api",
            audience: _configuration["Jwt:Audience"] ?? "pos-client",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
