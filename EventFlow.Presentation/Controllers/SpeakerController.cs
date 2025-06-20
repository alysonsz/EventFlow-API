﻿using EventFlow.Core.Models;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace EventFlow.Presentation.Controllers;

[Route("speaker")]
[ApiController]
public class SpeakerController(ISpeakerService speakerService) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] SpeakerCommand speakerCommand)
    {
        try
        {
            var speaker = await speakerService.CreateAsync(speakerCommand);

            return speaker != null ?
                Ok(speaker) :
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
    [HttpPost("{speakerId:int}/event/{eventId:int}")]
    public async Task<IActionResult> RegisterToEventAsync(int speakerId, int eventId)
    {
        try
        {
            var success = await speakerService.RegisterToEventAsync(eventId, speakerId);

            if (!success)
                return NotFound(new[] { "Evento ou Palestrante não encontrado." });

            return Ok(new { message = "Palestrante vinculado com sucesso ao evento." });
        }
        catch (SqlException ex)
        {
            return StatusCode(500, new[] { "Erro ao acessar o banco de dados.", ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new[] { "Erro inesperado.", ex.Message });
        }
    }

    [Authorize]
    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] SpeakerCommand speakerCommand)
    {
        try
        {
            var updated = await speakerService.UpdateAsync(id, speakerCommand);
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
            var deleted = await speakerService.DeleteAsync(id);
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
    public async Task<IActionResult> GetSpeakerByIdAsync(int id)
    {
        try
        {
            var result = await speakerService.GetByIdAsync(id);
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
    public async Task<IActionResult> GetAllSpeakersAsync([FromQuery] QueryParameters queryParameters)
    {
        try
        {
            var result = await speakerService.GetAllSpeakersAsync(queryParameters);

            if (result.Items.Count == 0)
                return NotFound("Nenhum palestrante encontrado com os critérios fornecidos.");

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
