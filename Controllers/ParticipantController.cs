using EventFlow_API.Commands;
using EventFlow_API.Models;
using EventFlow_API.Services;
using EventFlow_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EventFlow_API.Controllers;

[Route("participant")]
[ApiController]
public class ParticipantController(IParticipantService participantService) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] ParticipantCommand participantCommand)
    {
        try
        {
            var participant = await participantService.CreateAsync(participantCommand);

            return participant != null ? 
                Ok(participant) : 
                BadRequest();
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
    [HttpPost("{eventId}/participant/{participantId}")]
    public async Task<IActionResult> RegisterParticipantAsync(int eventId, int participantId)
    {
        try
        {
            var success = await participantService.RegisterToEventAsync(eventId, participantId);
            return success
                ? Ok(new { message = "Participante vinculado ao evento com sucesso." })
                : NotFound(new { error = "Evento ou participante não encontrado." });
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
    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] ParticipantCommand participantCommand)
    {
        try
        {
            var updated = await participantService.UpdateAsync(id, participantCommand);
            return updated != null ? 
                Ok(updated) : 
                NotFound();
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
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            var deleted = await participantService.DeleteAsync(id);
            return deleted ? 
                Ok(id) : 
                NotFound();
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetParticipantByIdAsync(int id)
    {
        try
        {
            var result = await participantService.GetByIdAsync(id);
            return result != null ? 
                Ok(result) : 
                NotFound();
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

    [HttpGet("all")]
    public async Task<IActionResult> GetAllParticipantsAsync(int eventId)
    {
        try
        {
            var result = await participantService.GetAllAsync(eventId);
            return result.Count != 0 ? 
                Ok(result) : 
                NotFound();
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
