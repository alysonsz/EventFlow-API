using Microsoft.AspNetCore.Http;

namespace EventFlow_API.Tests.Controllers;

public class EventControllerTests
{
    private readonly Mock<IEventService> _serviceMock;
    private readonly EventController _controller;

    public EventControllerTests()
    {
        _serviceMock = new Mock<IEventService>();
        _controller = new EventController(_serviceMock.Object);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public async Task PostAsync_ReturnsOk_WhenEventCreated()
    {
        var command = new EventCommand { Title = "Test", Description = "Test Desc", Date = DateTime.Now, Location = "Test Location", OrganizerId = 1 };
        var createdEvent = new Event { Id = 1, Title = "Test" };

        _serviceMock.Setup(s => s.CreateAsync(command)).ReturnsAsync(createdEvent);

        var result = await _controller.PostAsync(command);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedEvent = Assert.IsType<Event>(okResult.Value);
        Assert.Equal(1, returnedEvent.Id);
    }

    [Fact]
    public async Task PostAsync_ReturnsBadRequest_WhenCreationFails()
    {
        var command = new EventCommand { Title = "Test", Description = "Test Desc", Date = DateTime.Now, Location = "Test Location", OrganizerId = 1 };
        _serviceMock.Setup(s => s.CreateAsync(command)).ReturnsAsync((Event)null!);

        var result = await _controller.PostAsync(command);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsOk_WhenUpdated()
    {
        var command = new EventCommand { Title = "Updated", Description = "Desc", Date = DateTime.Now, Location = "Loc", OrganizerId = 1 };
        var updatedEvent = new EventDTO { Id = 1, Title = "Updated" };

        _serviceMock.Setup(s => s.UpdateAsync(1, command)).ReturnsAsync(updatedEvent);

        var result = await _controller.UpdateAsync(1, command);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedEvent = Assert.IsType<EventDTO>(okResult.Value);
        Assert.Equal(1, returnedEvent.Id);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNotFound_WhenUpdateFails()
    {
        var command = new EventCommand { Title = "Updated" };
        _serviceMock.Setup(s => s.UpdateAsync(1, command)).ReturnsAsync((EventDTO)null!);

        var result = await _controller.UpdateAsync(1, command);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsOk_WhenDeleted()
    {
        _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _controller.DeleteAsync(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(1, okResult.Value);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNotFound_WhenDeleteFails()
    {
        _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

        var result = await _controller.DeleteAsync(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenEventExists()
    {
        var eventId = 1;
        var eventDto = new EventDTO { Id = eventId, Title = "Test Event" };
        _serviceMock.Setup(s => s.GetByIdAsync(eventId)).ReturnsAsync(eventDto);

        var result = await _controller.GetEventByIdAsync(eventId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<EventDTO>(okResult.Value);
        Assert.Equal(eventDto.Id, returnValue.Id);
    }

    [Fact]
    public async Task GetAllEventsAsync_ReturnsNotFound_WhenNoEventsExist()
    {
        var queryParameters = new QueryParameters();
        var emptyList = new List<EventDTO>();

        var pagedResult = new PagedResult<EventDTO>(emptyList, 1, 10, 0);

        _serviceMock
            .Setup(s => s.GetAllEventsAsync(It.IsAny<QueryParameters>()))
            .ReturnsAsync(pagedResult);

        var result = await _controller.GetAllEventsAsync(queryParameters);

        Assert.IsType<NotFoundResult>(result);
    }
}
