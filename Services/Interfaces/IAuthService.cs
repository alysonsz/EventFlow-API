using EventFlow_API.Commands;
using EventFlow_API.DTOs;
using EventFlow_API.Models.DTOs;
using System.Security.Claims;

namespace EventFlow_API.Services.Interfaces;

public interface IAuthService
{
    Task<UserDTO?> RegisterAsync(RegisterUserCommand command);
    Task<string?> LoginAsync(LoginUserCommand command);
    Task<UserDTO?> GetAuthenticatedUserAsync(ClaimsPrincipal user);
    Task<bool> UpdatePasswordAsync(ClaimsPrincipal user, UserPasswordUpdateDTO dto);
}
