using EventFlow.Application.DTOs;
using EventFlow.Application.Features.Organizers.Queries.GetOrganizerById;
using EventFlow.Core.Models;
using EventFlow.Core.Primitives;
using EventFlow.Core.Repository;
using Moq;
using AutoMapper;

namespace EventFlow.Tests.Handlers;

public class GetOrganizerByIdQueryHandlerTests
{
    private readonly Mock<IOrganizerRepository> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ICacheService> _mockCache;
    private readonly GetOrganizerByIdQueryHandler _handler;

    public GetOrganizerByIdQueryHandlerTests()
    {
        _mockRepo = new Mock<IOrganizerRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockCache = new Mock<ICacheService>();
        _handler = new GetOrganizerByIdQueryHandler(_mockRepo.Object, _mockMapper.Object, _mockCache.Object);
    }

    [Fact]
    public async Task Handle_ExistingOrganizer_ReturnsOrganizerDto()
    {
        var organizer = Organizer.Create("John Doe", "john@example.com");
        var organizerDto = new OrganizerDTO { Id = 1, Name = "John Doe", Email = "john@example.com" };
        var query = new GetOrganizerByIdQuery(1);

        _mockCache.Setup(c => c.GetAsync<OrganizerDTO>("organizer-1")).ReturnsAsync((OrganizerDTO?)null);
        _mockRepo.Setup(r => r.GetOrganizerByIdAsync(1)).ReturnsAsync(organizer);
        _mockMapper.Setup(m => m.Map<OrganizerDTO>(organizer)).Returns(organizerDto);
        _mockCache.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<OrganizerDTO>(), It.IsAny<TimeSpan>())).Returns(Task.CompletedTask);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.Id);
    }

    [Fact]
    public async Task Handle_NonExistingOrganizer_ReturnsNotFound()
    {
        var query = new GetOrganizerByIdQuery(999);
        _mockCache.Setup(c => c.GetAsync<OrganizerDTO>("organizer-999")).ReturnsAsync((OrganizerDTO?)null);
        _mockRepo.Setup(r => r.GetOrganizerByIdAsync(999)).ReturnsAsync((Organizer?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
    }
}
