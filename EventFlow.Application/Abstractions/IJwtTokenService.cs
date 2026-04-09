using System.Security.Claims;

namespace EventFlow.Application.Abstractions;

public interface IJwtTokenService
{
    string GenerateToken(int userId, string username, string email);
    ClaimsPrincipal? ValidateToken(string token);
}
