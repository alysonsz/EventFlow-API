using EventNucleus.Application.DTOs;
using EventNucleus.Application.Features.Events.Queries.GetEventById;
using EventNucleus.Core.Models;
using EventNucleus.Core.Primitives;
using EventNucleus.Core.Repository;
using Moq;
using AutoMapper;

namespace EventNucleus.Tests.Handlers;

public class GetEventByIdQueryHandlerTests
{
    private readonly Mock<IEventRepository> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ICacheService> _mockCache;
    private readonly GetEventByIdQueryHandler _handler;

    public GetEventByIdQueryHandlerTests()
    {
        _mockRepo = new Mock<IEventRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockCache = new Mock<ICacheService>();
        _handler = new GetEventByIdQueryHandler(_mockRepo.Object, _mockMapper.Object, _mockCache.Object);
    }

    [Fact]
    public async Task Handle_CachedResult_ReturnsCachedValue()
    {
        var cachedDto = new EventDTO { Id = 1, Title = "Cached Event" };
        var query = new GetEventByIdQuery(1);

        _mockCache.Setup(c => c.GetAsync<EventDTO>("event-1")).ReturnsAsync(cachedDto);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(cachedDto.Id, result.Value.Id);
        _mockRepo.Verify(r => r.GetEventByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Handle_NotCached_ReturnsFromRepository()
    {
        var existingEvent = Event.Create("Title", "Desc", DateTime.UtcNow.AddDays(2), "Location", 1);
        var eventDto = new EventDTO { Id = 1, Title = "Title" };
        var query = new GetEventByIdQuery(1);

        _mockCache.Setup(c => c.GetAsync<EventDTO>("event-1")).ReturnsAsync((EventDTO?)null);
        _mockRepo.Setup(r => r.GetEventWithDetailsByIdAsync(1)).ReturnsAsync(existingEvent);
        _mockMapper.Setup(m => m.Map<EventDTO>(existingEvent)).Returns(eventDto);
        _mockCache.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<EventDTO>(), It.IsAny<TimeSpan>())).Returns(Task.CompletedTask);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.Id);
        _mockCache.Verify(c => c.SetAsync("event-1", eventDto, It.IsAny<TimeSpan>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingEvent_ReturnsNotFound()
    {
        var query = new GetEventByIdQuery(999);
        _mockCache.Setup(c => c.GetAsync<EventDTO>("event-999")).ReturnsAsync((EventDTO?)null);
        _mockRepo.Setup(r => r.GetEventByIdAsync(999)).ReturnsAsync((Event?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
    }
}

