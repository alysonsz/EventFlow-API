using System.Security.Claims;

namespace EventFlow.Application.Services;

public interface IAuthService
{
    Task<UserDTO?> RegisterAsync(RegisterUserCommand command);
    Task<string?> LoginAsync(LoginUserCommand command);
    Task<UserDTO?> GetAuthenticatedUserAsync(ClaimsPrincipal user);
    Task<bool> UpdatePasswordAsync(ClaimsPrincipal user, UserPasswordUpdateDto dto);
}
