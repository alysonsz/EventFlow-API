using System.Security.Claims;

namespace EventFlow.Application.Services;

public class AuthService(
    IUserRepository userRepository, 
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService) : IAuthService
{
    public async Task<UserDTO?> RegisterAsync(RegisterUserCommand command)
    {
        if (await userRepository.ExistsByEmailAsync(command.Email))
            return null;

        var user = User.Create(
            command.Username,
            command.Email,
            passwordHasher.HashPassword(command.Password));

        await userRepository.AddAsync(user);

        return new UserDTO
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        };
    }

    public async Task<string?> LoginAsync(LoginUserCommand command)
    {
        var user = await userRepository.GetByEmailAsync(command.Email);

        if (user == null || !passwordHasher.VerifyPassword(command.Password, user!.PasswordHash))
            return null;

        return jwtTokenService.GenerateToken(user.Id, user.Username, user.Email);
    }

    public async Task<UserDTO?> GetAuthenticatedUserAsync(ClaimsPrincipal userClaims)
    {
        var email = userClaims.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrEmpty(email))
            return null;

        var entity = await userRepository.GetByEmailAsync(email);

        if (entity == null)
            return null;

        return new UserDTO
        {
            Id = entity.Id,
            Username = entity.Username,
            Email = entity.Email
        };
    }

    public async Task<bool> UpdatePasswordAsync(ClaimsPrincipal userClaims, UserPasswordUpdateDto dto)
    {
        var email = userClaims.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email)) return false;

        var entity = await userRepository.GetByEmailAsync(email);
        if (entity == null) return false;

        if (!passwordHasher.VerifyPassword(dto.CurrentPassword, entity.PasswordHash))
            return false;

        entity.UpdatePassword(passwordHasher.HashPassword(dto.NewPassword));

        await userRepository.UpdateAsync(entity);
        return true;
    }
}