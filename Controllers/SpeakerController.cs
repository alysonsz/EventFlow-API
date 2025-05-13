using EventFlow_API.Commands;
using EventFlow_API.Models;
using EventFlow_API.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EventFlow_API.Controllers;

[Route("speaker")]
[ApiController]
public class SpeakerController(ISpeakerRepository speakerRepository) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] SpeakerCommand speakerCommand)
    {
        try
        {
            var speaker = new Speaker
            {
                Name = speakerCommand.Name,
                Email = speakerCommand.Email,
                Biography = speakerCommand.Biography,
                EventId = speakerCommand.EventId
            };

            var create = await speakerRepository.PostAsync(speaker);

            if (create != null)
                return Ok(create);

            else
                return BadRequest();
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

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] SpeakerCommand speakerCommand)
    {
        try
        {
            var speakerId = await speakerRepository.GetSpeakerByIdAsync(id);

            var speaker = new Speaker
            {
                Name = speakerCommand.Name,
                Email = speakerCommand.Email,
                Biography = speakerCommand.Biography,
                EventId = speakerCommand.EventId
            };

            var update = await speakerRepository.UpdateAsync(speaker);

            if (update != null)
                return Ok(update);

            else
                return NotFound();
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

    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            var delete = await speakerRepository.DeleteAsync(id);

            if (delete > 0)
                return Ok(delete);
            else
                return NotFound();
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
            var result = await speakerRepository.GetSpeakerByIdAsync(id);

            if (result != null)
                return Ok(result);

            else
                return NotFound();
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
    public async Task<IActionResult> GetAllSpeakersAsync()
    {
        try
        {
            var result = await speakerRepository.GetAllSpeakersAsync();

            if (result != null)
                return Ok(result);

            else
                return NotFound();
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
