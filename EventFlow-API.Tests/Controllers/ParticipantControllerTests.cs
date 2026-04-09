using MediatR;
using Microsoft.AspNetCore.Http;
using EventFlow.Application.Features.Participants.Commands.CreateParticipant;
using EventFlow.Application.Features.Participants.Commands.DeleteParticipant;
using EventFlow.Application.Features.Participants.Commands.UpdateParticipant;
using EventFlow.Application.Features.Participants.Queries.GetParticipantById;
using EventFlow.Application.Features.Participants.Queries.GetParticipantsByEventId;
using EventFlow.Core.Primitives;

namespace EventFlow_API.Tests.Controllers;

public class ParticipantControllerTests
{
    private readonly ParticipantController _controller;
    private readonly Mock<ISender> _mockSender;

    public ParticipantControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _controller = new ParticipantController(_mockSender.Object);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public async Task PostAsync_ReturnsOk_WhenParticipantCreated()
    {
        var command = new CreateParticipantCommand("Test", "test@example.com", "Interests");
        var result = Result<int>.Success(1);

        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.PostAsync(command);

        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal(1, okResult.Value);
    }

    [Fact]
    public async Task PostAsync_ReturnsInternalServerError_WhenCreationFails()
    {
        var command = new CreateParticipantCommand("Test", "test@example.com", "Interests");
        var result = Result<int>.Failure(Error.Failure("Participant.CreateFailed", "Failed to create participant"));

        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.PostAsync(command);

        var objectResult = Assert.IsType<ObjectResult>(actionResult);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsOk_WhenUpdated()
    {
        var command = new UpdateParticipantCommand(1, "Updated", "updated@example.com", "Interests");
        var result = Result.Success();

        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.UpdateAsync(1, command);

        Assert.IsType<OkResult>(actionResult);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNotFound_WhenUpdateFails()
    {
        var command = new UpdateParticipantCommand(1, "Updated", "updated@example.com", "Interests");
        var result = Result.Failure(Error.NotFound("Participant.NotFound", "Participant not found"));

        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.UpdateAsync(1, command);

        Assert.IsType<NotFoundObjectResult>(actionResult);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsOk_WhenDeleted()
    {
        var command = new DeleteParticipantCommand(1);
        var result = Result.Success();

        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.DeleteAsync(1);

        Assert.IsType<OkResult>(actionResult);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNotFound_WhenDeleteFails()
    {
        var command = new DeleteParticipantCommand(1);
        var result = Result.Failure(Error.NotFound("Participant.NotFound", "Participant not found"));

        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.DeleteAsync(1);

        Assert.IsType<NotFoundObjectResult>(actionResult);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenParticipantExists()
    {
        var query = new GetParticipantByIdQuery(1);
        var participant = new ParticipantDTO { Id = 1, Name = "Participant 1" };
        var result = Result<ParticipantDTO>.Success(participant);

        _mockSender.Setup(s => s.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.GetParticipantByIdAsync(1);

        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        var returnValue = Assert.IsType<ParticipantDTO>(okResult.Value);
        returnValue.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithList()
    {
        var queryParameters = new QueryParameters();
        var query = new GetParticipantsByEventIdQuery(1, queryParameters);
        var participants = new List<ParticipantDTO> { new() { Id = 1, Name = "Participant 1" } };
        var pagedResult = new PagedResult<ParticipantDTO>(participants, 1, 10, 1);

        _mockSender.Setup(s => s.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync(pagedResult);

        var actionResult = await _controller.GetAllParticipantsAsync(1, queryParameters);

        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        var returnValue = Assert.IsType<List<ParticipantDTO>>(okResult.Value);
        returnValue.Should().HaveCount(1);
    }
}
