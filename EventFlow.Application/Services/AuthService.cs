using EventFlow.Core.Models.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace EventFlow.Application.Services;

public class AuthService(IUserRepository userRepository, IConfiguration configuration) : IAuthService
{
    public async Task<UserDTO?> RegisterAsync(RegisterUserCommand command)
    {
        if (await userRepository.ExistsByEmailAsync(command.Email))
            return null;

        var user = new User
        {
            Username = command.Username,
            Email = command.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(command.Password)
        };

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

        if (user == null || !BCrypt.Net.BCrypt.Verify(command.Password, user!.PasswordHash))
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(4),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
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

    public async Task<bool> UpdatePasswordAsync(ClaimsPrincipal userClaims, UserPasswordUpdateDTO dto)
    {
        var email = userClaims.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email)) return false;

        var entity = await userRepository.GetByEmailAsync(email);
        if (entity == null) return false;

        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, entity.PasswordHash))
            return false;

        entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        await userRepository.UpdateAsync(entity);
        return true;
    }
}