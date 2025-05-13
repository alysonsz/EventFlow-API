using EventFlow_API.Commands;
using EventFlow_API.Models;
using EventFlow_API.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EventFlow_API.Controllers;

[Route("event")]
[ApiController]
public class EventController(IEventRepository eventRepository) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] EventCommand eventCommand)
    {
        try
        {
            var eventModel = new Event
            {
                Title = eventCommand.Title,
                Location = eventCommand.Location,
                Organizer = eventCommand.Organizer,
                Description = eventCommand.Description,
                Date = eventCommand.Date,
                OrganizerId = eventCommand.OrganizerId,
                Speakers = eventCommand.Speakers,
                Participants = eventCommand.Participants
            };

            var create = await eventRepository.PostAsync(eventModel);

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
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] EventCommand eventCommand)
    {
        try
        {
            var eventId = await eventRepository.GetEventByIdAsync(id);

            var eventModel = new Event
            {
                Id = id,
                Title = eventCommand.Title,
                Location = eventCommand.Location,
                Organizer = eventCommand.Organizer,
                Description = eventCommand.Description,
                Date = eventCommand.Date,
                OrganizerId = eventCommand.OrganizerId,
                Speakers = eventCommand.Speakers,
                Participants = eventCommand.Participants
            };

            var update = await eventRepository.UpdateAsync(eventModel);

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
            var delete = await eventRepository.DeleteAsync(id);

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
    public async Task<IActionResult> GetEventByIdAsync(int id)
    {
        try
        {
            var result = await eventRepository.GetEventByIdAsync(id);

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
    public async Task<IActionResult> GetAllEventsAsync()
    {
        try
        {
            var result = await eventRepository.GetAllEventsAsync();

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
