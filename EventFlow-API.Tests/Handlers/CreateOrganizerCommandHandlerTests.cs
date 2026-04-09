using EventFlow.Application.Features.Organizers.Commands.CreateOrganizer;
using EventFlow.Core.Models;
using EventFlow.Core.Repository;
using Moq;

namespace EventFlow.Tests.Handlers;

public class CreateOrganizerCommandHandlerTests
{
    private readonly Mock<IOrganizerRepository> _mockRepo;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CreateOrganizerCommandHandler _handler;

    public CreateOrganizerCommandHandlerTests()
    {
        _mockRepo = new Mock<IOrganizerRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new CreateOrganizerCommandHandler(_mockRepo.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccessWithOrganizerId()
    {
        var command = new CreateOrganizerCommand("John Doe", "john@example.com");

        _mockRepo.Setup(r => r.PostAsync(It.IsAny<Organizer>())).ReturnsAsync(Organizer.Create("John Doe", "john@example.com"));
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        _mockRepo.Verify(r => r.PostAsync(It.IsAny<Organizer>()), Times.Once);
    }
}
