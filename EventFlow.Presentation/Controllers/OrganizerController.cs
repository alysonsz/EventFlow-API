using EventFlow.Core.Models;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace EventFlow.Presentation.Controllers;

[Route("organizer")]
[ApiController]
public class OrganizerController(IOrganizerService organizerService) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] OrganizerCommand organizerCommand)
    {
        try
        {
            var organizer = await organizerService.CreateAsync(organizerCommand);

            return organizer != null ?
                Ok(organizer) :
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
    [HttpPost("{organizerId}/event/{eventId}")]
    public async Task<IActionResult> RegisterParticipantAsync(int organizerId, int eventId)
    {
        try
        {
            var success = await organizerService.RegisterToEventAsync(organizerId, eventId);

            return success ?
                Ok(new { message = "Evento vinculado ao organizador com sucesso." }) :
                NotFound(new { error = "Organizador ou evento não encontrado." });
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
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] OrganizerCommand organizerCommand)
    {
        try
        {
            var updated = await organizerService.UpdateAsync(id, organizerCommand);
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
            var deleted = await organizerService.DeleteAsync(id);
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
    public async Task<IActionResult> GetOrganizerByIdAsync(int id)
    {
        try
        {
            var result = await organizerService.GetByIdAsync(id);
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
    public async Task<IActionResult> GetAllOrganizersAsync([FromQuery] QueryParameters queryParameters)
    {
        try
        {
            var result = await organizerService.GetAllPagedOrganizersAsync(queryParameters);

            if (result.Items.Count == 0)
                return NotFound("Nenhum organizador encontrado com os critérios fornecidos.");

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
