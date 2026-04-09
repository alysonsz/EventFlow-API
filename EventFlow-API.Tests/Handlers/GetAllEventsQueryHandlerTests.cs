using EventFlow.Application.DTOs;
using EventFlow.Application.Features.Events.Queries.GetAllEvents;
using EventFlow.Core.Models;
using EventFlow.Core.Repository;
using Moq;
using AutoMapper;

namespace EventFlow.Tests.Handlers;

public class GetAllEventsQueryHandlerTests
{
    private readonly Mock<IEventRepository> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAllEventsQueryHandler _handler;

    public GetAllEventsQueryHandlerTests()
    {
        _mockRepo = new Mock<IEventRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetAllEventsQueryHandler(_mockRepo.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ReturnsPagedResult()
    {
        var events = new List<Event>
        {
            Event.Create("Event 1", "Desc 1", DateTime.UtcNow.AddDays(2), "Location 1", 1),
            Event.Create("Event 2", "Desc 2", DateTime.UtcNow.AddDays(3), "Location 2", 1)
        };

        var pagedResult = new PagedResult<Event>(events, 1, 10, 2);
        var eventDtos = new List<EventDTO>
        {
            new() { Id = 1, Title = "Event 1" },
            new() { Id = 2, Title = "Event 2" }
        };

        var query = new GetAllEventsQuery(new QueryParameters { PageNumber = 1, PageSize = 10 });

        _mockRepo.Setup(r => r.GetAllPagedEventsAsync(It.IsAny<QueryParameters>())).ReturnsAsync(pagedResult);
        _mockMapper.Setup(m => m.Map<List<EventDTO>>(events)).Returns(eventDtos);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Equal(2, result.Items.Count);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(2, result.TotalCount);
    }

    [Fact]
    public async Task Handle_EmptyResult_ReturnsEmptyList()
    {
        var pagedResult = new PagedResult<Event>(new List<Event>(), 1, 10, 0);
        var query = new GetAllEventsQuery(new QueryParameters { PageNumber = 1, PageSize = 10 });

        _mockRepo.Setup(r => r.GetAllPagedEventsAsync(It.IsAny<QueryParameters>())).ReturnsAsync(pagedResult);
        _mockMapper.Setup(m => m.Map<List<EventDTO>>(It.IsAny<List<Event>>())).Returns(new List<EventDTO>());

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }
}
