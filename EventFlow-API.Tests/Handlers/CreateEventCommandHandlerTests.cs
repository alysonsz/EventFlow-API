using EventFlow.Application.Features.Events.Commands.CreateEvent;
using EventFlow.Core.Models;
using EventFlow.Core.Primitives;
using EventFlow.Core.Repository;
using Moq;

namespace EventFlow.Tests.Handlers;

public class CreateEventCommandHandlerTests
{
    private readonly Mock<IEventRepository> _mockRepo;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CreateEventCommandHandler _handler;

    public CreateEventCommandHandlerTests()
    {
        _mockRepo = new Mock<IEventRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new CreateEventCommandHandler(_mockRepo.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccessWithEventId()
    {
        var futureDate = DateTime.UtcNow.AddDays(2);
        var command = new CreateEventCommand(
            "Test Event",
            "Description",
            futureDate,
            "Location",
            1);

        _mockRepo.Setup(r => r.PostAsync(It.IsAny<Event>()))
            .ReturnsAsync((Event e) => e);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.True(result.Value >= 0);
        _mockRepo.Verify(r => r.PostAsync(It.IsAny<Event>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_PastDate_ThrowsBusinessRuleException()
    {
        var command = new CreateEventCommand(
            "Test Event",
            "Description",
            DateTime.UtcNow.AddDays(-1),
            "Location",
            1);

        await Assert.ThrowsAsync<BusinessRuleValidationException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
