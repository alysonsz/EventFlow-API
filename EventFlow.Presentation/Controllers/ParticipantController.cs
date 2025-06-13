using EventFlow.Core.Models;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace EventFlow.Presentation.Controllers;

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

    [HttpGet("{eventId}/all")]
    public async Task<IActionResult> GetAllParticipantsAsync(int eventId, [FromQuery] QueryParameters queryParameters)
    {
        try
        {
            var result = await participantService.GetAllParticipantsByEventIdAsync(eventId, queryParameters);

            if (result.Items.Count == 0)
                return NotFound("Nenhum participante encontrado para este evento com os critérios fornecidos.");

            var metadata = new
            {
                result.TotalCount,
                result.PageSize,
                result.PageNumber,
                result.TotalPages,
                result.HasNextPage,
                result.HasPreviousPage
            };
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metadata));

            return Ok(result.Items);
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
