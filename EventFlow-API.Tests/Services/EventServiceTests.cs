using AutoMapper;
using EventFlow.Core.Models;
using EventFlow.Core.Repository.Interfaces;

namespace EventFlow_API.Tests.Services;

public class EventServiceTests
{
    private readonly Mock<IEventRepository> _eventRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly EventService _eventService;

    public EventServiceTests()
    {
        _eventRepoMock = new Mock<IEventRepository>();
        _mapperMock = new Mock<IMapper>();
        _eventService = new EventService(_eventRepoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnEvent_WhenCreated()
    {
        var command = new EventCommand
        {
            Title = "Test",
            Date = DateTime.Now,
            Location = "Loc",
            OrganizerId = 1
        };

        var newEvent = new Event
        {
            Id = 1,
            Title = "Test"
        };

        _eventRepoMock.Setup(r => r.PostAsync(It.IsAny<Event>())).ReturnsAsync(newEvent);
        _eventRepoMock.Setup(r => r.GetEventWithDetailsByIdAsync(newEvent.Id)).ReturnsAsync(newEvent);

        var result = await _eventService.CreateAsync(command);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnNull_WhenPostFails()
    {
        var command = new EventCommand
        {
            Title = "Test"
        };

        _eventRepoMock.Setup(r => r.PostAsync(It.IsAny<Event>())).ReturnsAsync((Event)null!);

        var result = await _eventService.CreateAsync(command);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedEvent_WhenUpdateSuccessful()
    {
        var command = new EventCommand
        {
            Title = "Updated",
            Date = DateTime.Now,
            Location = "Loc",
            OrganizerId = 1
        };
        var existing = new Event
        {
            Id = 1
        };

        var updated = new Event
        {
            Id = 1,
            Title = "Updated"
        };

        var updatedDto = new EventDTO
        {
            Id = 1,
            Title = "Updated"
        };

        _eventRepoMock.Setup(r => r.GetEventByIdAsync(1)).ReturnsAsync(existing);
        _eventRepoMock.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(updated);
        _mapperMock.Setup(m => m.Map<EventDTO>(updated)).Returns(updatedDto);

        var result = await _eventService.UpdateAsync(1, command);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenEventNotFound()
    {
        _eventRepoMock.Setup(r => r.GetEventByIdAsync(1)).ReturnsAsync((Event)null!);

        var result = await _eventService.UpdateAsync(1, new EventCommand());

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenUpdateFails()
    {
        var command = new EventCommand
        {
            Title = "Updated"
        };

        var existing = new Event
        {
            Id = 1
        };

        _eventRepoMock.Setup(r => r.GetEventByIdAsync(1)).ReturnsAsync(existing);
        _eventRepoMock.Setup(r => r.UpdateAsync(existing)).ReturnsAsync((Event)null!);

        var result = await _eventService.UpdateAsync(1, command);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenDeleted()
    {
        _eventRepoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(1);

        var result = await _eventService.DeleteAsync(1);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenDeleteFails()
    {
        _eventRepoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(0);

        var result = await _eventService.DeleteAsync(1);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEventDTO_WhenEventExists()
    {
        var eventId = 1;
        var eventEntity = new Event
        {
            Id = eventId,
            Title = "Event Title"
        };

        var eventDto = new EventDTO
        {
            Id = eventId,
            Title = "Event Title"
        };

        _eventRepoMock.Setup(r => r.GetEventWithDetailsByIdAsync(eventId)).ReturnsAsync(eventEntity);
        _mapperMock.Setup(m => m.Map<EventDTO>(eventEntity)).Returns(eventDto);

        var result = await _eventService.GetByIdAsync(eventId);

        result.Should().NotBeNull();
        result!.Id.Should().Be(eventId);
        result.Title.Should().Be("Event Title");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenEventDoesNotExist()
    {
        var eventId = 1;

        _eventRepoMock.Setup(r => r.GetEventWithDetailsByIdAsync(eventId)).ReturnsAsync((Event)null!);

        var result = await _eventService.GetByIdAsync(eventId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedEvents()
    {
        var queryParameters = new QueryParameters
        {
            PageNumber = 1,
            PageSize = 10,
            Filter = null,
            SortBy = null
        };

        var events = new List<Event> 
        { 
            new Event
            {
                Id = 1,
                Title = "Test"
            }
        };

        var pagedResult = new PagedResult<Event>(events, queryParameters.PageNumber, queryParameters.PageSize, totalCount: 1);

        var eventsDto = new List<EventDTO> 
        { 
            new EventDTO
            {
                Id = 1,
                Title = "Test"
            }
        };

        var pagedResultDto = new PagedResult<EventDTO>(eventsDto, queryParameters.PageNumber, queryParameters.PageSize, totalCount: 1);

        _eventRepoMock.Setup(r => r.GetAllPagedEventsAsync(queryParameters)).ReturnsAsync(pagedResult);
        _mapperMock.Setup(m => m.Map<List<EventDTO>>(events)).Returns(eventsDto);

        var result = await _eventService.GetAllPagedEventsAsync(queryParameters);

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Items.First().Title.Should().Be("Test");
        result.TotalCount.Should().Be(1);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
    }
}
