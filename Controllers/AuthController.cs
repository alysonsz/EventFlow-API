using EventFlow_API.Commands;
using EventFlow_API.DTOs;
using EventFlow_API.Models.DTOs;
using EventFlow_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace EventFlow_API.Controllers;

[Route("authentication")]
[ApiController]
public class AuthController(IAuthService authService) : Controller
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        try
        {
            var result = await authService.RegisterAsync(command);
            if (result == null)
                return BadRequest(new[] { "Este e-mail já está cadastrado." });

            return Ok(result);
        }
        catch (SqlException error)
        {
            return StatusCode(500,
                new[] { "Não foi possível conectar ao banco de dados, por favor tente mais tarde", error.Message });
        }
        catch (DbUpdateException error)
        {
            return StatusCode(500,
                new[] { "Algo de errado aconteceu ao salvar, por favor tente mais tarde", error.Message });
        }
        catch (Exception error)
        {
            return StatusCode(500,
                new[] { error.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        try
        {
            var token = await authService.LoginAsync(command);
            if (string.IsNullOrEmpty(token))
                return Unauthorized(new[] { "E-mail ou senha inválidos. " });

            return Ok(new { token });
        }
        catch (SqlException error)
        {
            return StatusCode(500,
                new[] { "Não foi possível conectar ao banco de dados, por favor tente mais tarde", error.Message });
        }
        catch (DbUpdateException error)
        {
            return StatusCode(500,
                new[] { "Algo de errado aconteceu ao salvar, por favor tente mais tarde", error.Message });
        }
        catch (Exception error)
        {
            return StatusCode(500,
                new[] { error.Message });
        }
    }

    [Authorize]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] UserPasswordUpdateDTO dto)
    {
        try
        {
            var result = await authService.UpdatePasswordAsync(User, dto);

            if (!result)
                return BadRequest("Senha atual incorreta ou usuário inválido.");

            return Ok("Senha atualizada com sucesso.");
        }
        catch (SqlException error)
        {
            return StatusCode(500,
                new[] { "Não foi possível conectar ao banco de dados, por favor tente mais tarde", error.Message });
        }
        catch (DbUpdateException error)
        {
            return StatusCode(500,
                new[] { "Algo de errado aconteceu ao salvar, por favor tente mais tarde", error.Message });
        }
        catch (Exception error)
        {
            return StatusCode(500,
                new[] { error.Message });
        }
    }

    [Authorize]
    [HttpGet("data")]
    public async Task<IActionResult> GetAuthenticatedUserAsync()
    {
        try
        {
            var userDto = await authService.GetAuthenticatedUserAsync(User);

            if (userDto == null)
                return Unauthorized("Usuário não encontrado ou token inválido.");

            return Ok(userDto);
        }
        catch (SqlException error)
        {
            return StatusCode(500,
                new[] { "Não foi possível conectar ao banco de dados, por favor tente mais tarde", error.Message });
        }
        catch (DbUpdateException error)
        {
            return StatusCode(500,
                new[] { "Algo de errado aconteceu ao salvar, por favor tente mais tarde", error.Message });
        }
        catch (Exception error)
        {
            return StatusCode(500,
                new[] { error.Message });
        }
    }
}
