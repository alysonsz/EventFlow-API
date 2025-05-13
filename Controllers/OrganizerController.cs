using EventFlow_API.Commands;
using EventFlow_API.Models;
using EventFlow_API.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EventFlow_API.Controllers;

[Route("organizer")]
[ApiController]
public class OrganizerController(IOrganizerRepository organizerRepository) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] OrganizerCommand organizerCommand)
    {
        try
        {
            var organizer = new Organizer
            {
                Name = organizerCommand.Name,
                Email = organizerCommand.Email
            };

            var create = await organizerRepository.PostAsync(organizer);

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
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] OrganizerCommand organizerCommand)
    {
        try
        {
            var organizerId = await organizerRepository.GetOrganizerByIdAsync(id);
            var organizer = new Organizer
            {
                Name = organizerCommand.Name,
                Email = organizerCommand.Email
            };

            var update = await organizerRepository.UpdateAsync(organizer);

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
            var delete = await organizerRepository.DeleteAsync(id);

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
    public async Task<IActionResult> GetOrganizerByIdAsync(int id)
    {
        try
        {
            var result = await organizerRepository.GetOrganizerByIdAsync(id);

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
    public async Task<IActionResult> GetAllOrganizersAsync()
    {
        try
        {
            var result = await organizerRepository.GetAllOrganizersAsync();

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
