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

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync([FromBody] SpeakerCommand speakerCommand)
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
            var update = await speakerRepository.UpdateAsync(speaker);
            if (update != null)
                return Ok(update);

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

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAsync([FromBody] SpeakerCommand speakerCommand)
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
            var delete = await speakerRepository.DeleteAsync(speaker);
            if (delete != null)
                return Ok(delete);

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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSpeakerByIdAsync(int id)
    {
        try
        {
            var result = await speakerRepository.GetSpeakerByIdAsync(id);
            if (result != null)
                return Ok(result);

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

    [HttpGet("all/{id:int}")]
    public async Task<IActionResult> GetAllSpeakersAsync(int id)
    {
        try
        {
            var result = await speakerRepository.GetAllSpeakersAsync(id);
            if (result != null)
                return Ok(result);

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
}
