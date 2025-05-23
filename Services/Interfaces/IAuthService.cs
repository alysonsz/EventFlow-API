using EventFlow_API.Commands;
using EventFlow_API.Models.DTOs;

namespace EventFlow_API.Services.Interfaces;

public interface IAuthService
{
    Task<UserDTO?> RegisterAsync(RegisterUserCommand command);
    Task<string?> LoginAsync(LoginUserCommand command);
}
