using System.Security.Claims;

namespace EventNucleus.Application.Abstractions;

public interface IJwtTokenService
{
    string GenerateToken(int userId, string username, string email);
    ClaimsPrincipal? ValidateToken(string token);
}

