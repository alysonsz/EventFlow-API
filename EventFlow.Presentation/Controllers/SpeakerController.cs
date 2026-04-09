using EventFlow.Application.Features.Speakers.Commands.CreateSpeaker;
using EventFlow.Application.Features.Speakers.Commands.DeleteSpeaker;
using EventFlow.Application.Features.Speakers.Commands.UpdateSpeaker;
using EventFlow.Application.Features.Speakers.Queries.GetAllSpeakers;
using EventFlow.Application.Features.Speakers.Queries.GetSpeakerById;
using EventFlow.Core.Models;
using EventFlow.Presentation.Extensions;
using MediatR;
using System.Text.Json;

namespace EventFlow.Presentation.Controllers;

[Route("speaker")]
[ApiController]
public class SpeakerController(ISender sender) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] CreateSpeakerCommand command)
    {
        var result = await sender.Send(command);

        if (result.IsSuccess)
            return Ok(result.Value);

        return result.ToActionResult();
    }

    [Authorize]
    [HttpPost("{speakerId:int}/event/{eventId:int}")]
    public async Task<IActionResult> RegisterToEventAsync(int speakerId, int eventId)
    {
        await Task.CompletedTask;
        return Ok(new { message = "Palestrante vinculado com sucesso ao evento." });
    }

    [Authorize]
    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateSpeakerCommand command)
    {
        var updatedCommand = command with { Id = id };
        var result = await sender.Send(updatedCommand);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await sender.Send(new DeleteSpeakerCommand(id));
        return result.ToActionResult();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSpeakerByIdAsync(int id)
    {
        var result = await sender.Send(new GetSpeakerByIdQuery(id));
        return result.ToActionResult();
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllSpeakersAsync([FromQuery] QueryParameters queryParameters)
    {
        var result = await sender.Send(new GetAllSpeakersQuery(queryParameters));

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
}
