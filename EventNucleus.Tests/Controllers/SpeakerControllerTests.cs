using MediatR;
using Microsoft.AspNetCore.Http;
using EventNucleus.Application.Features.Speakers.Commands.CreateSpeaker;
using EventNucleus.Application.Features.Speakers.Commands.DeleteSpeaker;
using EventNucleus.Application.Features.Speakers.Commands.UpdateSpeaker;
using EventNucleus.Application.Features.Speakers.Queries.GetAllSpeakers;
using EventNucleus.Application.Features.Speakers.Queries.GetSpeakerById;
using EventNucleus.Core.Primitives;

namespace EventNucleus_API.Tests.Controllers;

public class SpeakerControllerTests
{
    private readonly SpeakerController _controller;
    private readonly Mock<ISender> _mockSender;

    public SpeakerControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _controller = new SpeakerController(_mockSender.Object);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public async Task PostAsync_ReturnsOk_WhenSpeakerCreated()
    {
        var command = new CreateSpeakerCommand("Test", "test@example.com", "Bio", "Expertise");
        var result = Result<int>.Success(1);

        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.PostAsync(command);

        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal(1, okResult.Value);
    }

    [Fact]
    public async Task PostAsync_ReturnsBadRequest_WhenCreationFails()
    {
        var command = new CreateSpeakerCommand("Test", "test@example.com", "Bio", "Expertise");
        var result = Result<int>.Failure(Error.Validation("Speaker.CreateFailed", "Failed to create speaker"));

        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.PostAsync(command);

        Assert.IsType<BadRequestObjectResult>(actionResult);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsOk_WhenUpdated()
    {
        var command = new UpdateSpeakerCommand(1, "Updated", "updated@example.com", "Bio", "Expertise");
        var result = Result.Success();

        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.UpdateAsync(1, command);

        Assert.IsType<OkResult>(actionResult);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNotFound_WhenUpdateFails()
    {
        var command = new UpdateSpeakerCommand(1, "Updated", "updated@example.com", "Bio", "Expertise");
        var result = Result.Failure(Error.NotFound("Speaker.NotFound", "Speaker not found"));

        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.UpdateAsync(1, command);

        Assert.IsType<NotFoundObjectResult>(actionResult);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsOk_WhenDeleted()
    {
        var command = new DeleteSpeakerCommand(1);
        var result = Result.Success();

        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.DeleteAsync(1);

        Assert.IsType<OkResult>(actionResult);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNotFound_WhenDeleteFails()
    {
        var command = new DeleteSpeakerCommand(1);
        var result = Result.Failure(Error.NotFound("Speaker.NotFound", "Speaker not found"));

        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.DeleteAsync(1);

        Assert.IsType<NotFoundObjectResult>(actionResult);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenSpeakerExists()
    {
        var query = new GetSpeakerByIdQuery(1);
        var speaker = new SpeakerDTO { Id = 1, Name = "Speaker 1" };
        var result = Result<SpeakerDTO>.Success(speaker);

        _mockSender.Setup(s => s.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.GetSpeakerByIdAsync(1);

        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        var returnValue = Assert.IsType<SpeakerDTO>(okResult.Value);
        returnValue.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithList()
    {
        var queryParameters = new QueryParameters();
        var query = new GetAllSpeakersQuery(queryParameters);
        var speakers = new List<SpeakerDTO> { new() { Id = 1, Name = "Speaker 1" } };
        var pagedResult = new PagedResult<SpeakerDTO>(speakers, 1, 10, 1);

        _mockSender.Setup(s => s.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync(pagedResult);

        var actionResult = await _controller.GetAllSpeakersAsync(queryParameters);

        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        var returnValue = Assert.IsType<List<SpeakerDTO>>(okResult.Value);
        returnValue.Should().HaveCount(1);
    }
}

