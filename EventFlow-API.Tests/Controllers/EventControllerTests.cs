using MediatR;
using Microsoft.AspNetCore.Http;
using EventFlow.Application.Features.Events.Commands.CreateEvent;
using EventFlow.Application.Features.Events.Commands.DeleteEvent;
using EventFlow.Application.Features.Events.Commands.UpdateEvent;
using EventFlow.Application.Features.Events.Queries.GetAllEvents;
using EventFlow.Application.Features.Events.Queries.GetEventById;
using EventFlow.Core.Primitives;

namespace EventFlow_API.Tests.Controllers;

public class EventControllerTests
{
    private readonly EventController _controller;
    private readonly Mock<IMediator> _mockMediator;

    public EventControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _controller = new EventController(_mockMediator.Object);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WhenEventCreated()
    {
        var command = new CreateEventCommand("Test Event", "Description", DateTime.Now.AddDays(1), "Location", 1);
        var result = Result<int>.Success(1);

        _mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.Create(command, CancellationToken.None);

        var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult);
        Assert.Equal(1, createdResult.RouteValues!["id"]);
    }

    [Fact]
    public async Task Create_ReturnsError_WhenCreationFails()
    {
        var command = new CreateEventCommand("Test Event", "Description", DateTime.Now.AddDays(1), "Location", 1);
        var result = Result<int>.Failure(Error.Validation("Event.CreateFailed", "Failed to create event"));

        _mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.Create(command, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(actionResult);
    }

    [Fact]
    public async Task Update_ReturnsOk_WhenUpdated()
    {
        var command = new UpdateEventCommand(1, "Updated Event", "Description", DateTime.Now.AddDays(1), "Location");
        var result = Result.Success();

        _mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.Update(1, command, CancellationToken.None);

        Assert.IsType<OkResult>(actionResult);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenEventNotFound()
    {
        var command = new UpdateEventCommand(1, "Updated Event", "Description", DateTime.Now.AddDays(1), "Location");
        var result = Result.Failure(Error.NotFound("Event.NotFound", "Event not found"));

        _mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.Update(1, command, CancellationToken.None);

        Assert.IsType<NotFoundObjectResult>(actionResult);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenDeleted()
    {
        var command = new DeleteEventCommand(1);
        var result = Result.Success();

        _mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.Delete(1, CancellationToken.None);

        Assert.IsType<OkResult>(actionResult);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenDeleteFails()
    {
        var command = new DeleteEventCommand(1);
        var result = Result.Failure(Error.NotFound("Event.NotFound", "Event not found"));

        _mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.Delete(1, CancellationToken.None);

        Assert.IsType<NotFoundObjectResult>(actionResult);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenEventExists()
    {
        var query = new GetEventByIdQuery(1);
        var evento = new EventDTO { Id = 1, Title = "Event 1" };
        var result = Result<EventDTO>.Success(evento);

        _mockMediator.Setup(m => m.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.GetById(1, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        var returnValue = Assert.IsType<EventDTO>(okResult.Value);
        returnValue.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithList()
    {
        var queryParameters = new QueryParameters();
        var query = new GetAllEventsQuery(queryParameters);
        var events = new List<EventDTO> { new() { Id = 1, Title = "Event 1" } };
        var pagedResult = new PagedResult<EventDTO>(events, 1, 10, 1);

        _mockMediator.Setup(m => m.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync(pagedResult);

        var actionResult = await _controller.GetAll(queryParameters, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        var returnValue = Assert.IsType<List<EventDTO>>(okResult.Value);
        returnValue.Should().HaveCount(1);
    }
}
