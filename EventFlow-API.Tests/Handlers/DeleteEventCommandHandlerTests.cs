using EventFlow.Application.Features.Events.Commands.DeleteEvent;
using EventFlow.Core.Models;
using EventFlow.Core.Primitives;
using EventFlow.Core.Repository;
using Moq;

namespace EventFlow.Tests.Handlers;

public class DeleteEventCommandHandlerTests
{
    private readonly Mock<IEventRepository> _mockRepo;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICacheService> _mockCache;
    private readonly DeleteEventCommandHandler _handler;

    public DeleteEventCommandHandlerTests()
    {
        _mockRepo = new Mock<IEventRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockCache = new Mock<ICacheService>();
        _handler = new DeleteEventCommandHandler(_mockRepo.Object, _mockUnitOfWork.Object, _mockCache.Object);
    }

    [Fact]
    public async Task Handle_ExistingEvent_ReturnsSuccess()
    {
        var existingEvent = Event.Create("Title", "Desc", DateTime.UtcNow.AddDays(2), "Location", 1);
        var command = new DeleteEventCommand(1);

        _mockRepo.Setup(r => r.GetEventByIdAsync(1)).ReturnsAsync(existingEvent);
        _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(1);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _mockCache.Setup(c => c.RemoveAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        _mockCache.Verify(c => c.RemoveAsync("event-1"), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingEvent_ReturnsNotFound()
    {
        var command = new DeleteEventCommand(999);
        _mockRepo.Setup(r => r.GetEventByIdAsync(999)).ReturnsAsync((Event?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
    }
}
