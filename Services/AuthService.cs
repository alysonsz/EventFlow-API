using EventFlow_API.Commands;
using EventFlow_API.Models;
using EventFlow_API.Models.DTOs;
using EventFlow_API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventFlow_API.Services;

public class AuthService(EventFlowContext context, IConfiguration configuration) : IAuthService
{
    public async Task<UserDTO?> RegisterAsync(RegisterUserCommand command)
    {
        if (await context.Users.AnyAsync(u => u.Email == command.Email))
            return null;

        var user = new User
        {
            Username = command.Username,
            Email = command.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(command.Password)
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserDTO
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        };
    }

    public async Task<string?> LoginAsync(LoginUserCommand command)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email == command.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(command.Password, user!.PasswordHash))
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            ]),

            Expires = DateTime.UtcNow.AddHours(4),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
