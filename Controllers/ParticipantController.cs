using EventFlow_API.Commands;
using EventFlow_API.Models;
using EventFlow_API.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EventFlow_API.Controllers;

[Route("participant")]
[ApiController]
public class ParticipantController(IParticipantRepository participantRepository) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] ParticipantCommand participantCommand)
    {
        try
        {
            var participant = new Participant
            {
                Name = participantCommand.Name,
                Email = participantCommand.Email,
                EventId = participantCommand.EventId
            };
            var create = await participantRepository.PostAsync(participant);
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
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] ParticipantCommand participantCommand)
    {
        try
        {
            var participantId = await participantRepository.GetParticipantByIdAsync(id);
            var participant = new Participant
            {
                Name = participantCommand.Name,
                Email = participantCommand.Email,
                EventId = participantCommand.EventId
            };

            var update = await participantRepository.UpdateAsync(participant);

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
            var delete = await participantRepository.DeleteAsync(id);
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
    public async Task<IActionResult> GetParticipantByIdAsync(int id)
    {
        try
        {
            var result = await participantRepository.GetParticipantByIdAsync(id);
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
    public async Task<IActionResult> GetAllParticipantsAsync()
    {
        try
        {
            var result = await participantRepository.GetAllParticipantsAsync();
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
