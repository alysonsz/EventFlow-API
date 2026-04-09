using MediatR;
using Microsoft.AspNetCore.Http;
using EventFlow.Application.Features.Organizers.Commands.CreateOrganizer;
using EventFlow.Application.Features.Organizers.Commands.DeleteOrganizer;
using EventFlow.Application.Features.Organizers.Commands.UpdateOrganizer;
using EventFlow.Application.Features.Organizers.Queries.GetAllOrganizers;
using EventFlow.Application.Features.Organizers.Queries.GetOrganizerById;
using EventFlow.Core.Primitives;

namespace EventFlow_API.Tests.Controllers;

public class OrganizerControllerTests
{
    private readonly OrganizerController _controller;
    private readonly Mock<ISender> _mockSender;

    public OrganizerControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _controller = new OrganizerController(_mockSender.Object);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public async Task PostAsync_ReturnsOk_WhenOrganizerCreated()
    {
        var command = new CreateOrganizerCommand("Test", "test@example.com");
        var result = Result<int>.Success(1);

        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.PostAsync(command);

        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal(1, okResult.Value);
    }

    [Fact]
    public async Task PostAsync_ReturnsBadRequest_WhenCreationFails()
    {
        var command = new CreateOrganizerCommand("Test", "test@example.com");
        var result = Result<int>.Failure(Error.Validation("Organizer.CreateFailed", "Failed to create organizer"));

        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.PostAsync(command);

        Assert.IsType<BadRequestObjectResult>(actionResult);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsOk_WhenUpdated()
    {
        var command = new UpdateOrganizerCommand(1, "Updated", "updated@example.com");
        var result = Result.Success();

        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.UpdateAsync(1, command);

        Assert.IsType<OkResult>(actionResult);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNotFound_WhenOrganizerNotFound()
    {
        var command = new UpdateOrganizerCommand(1, "Updated", "updated@example.com");
        var result = Result.Failure(Error.NotFound("Organizer.NotFound", "Organizer not found"));

        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.UpdateAsync(1, command);

        Assert.IsType<NotFoundObjectResult>(actionResult);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsOk_WhenDeleted()
    {
        var command = new DeleteOrganizerCommand(1);
        var result = Result.Success();

        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.DeleteAsync(1);

        Assert.IsType<OkResult>(actionResult);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNotFound_WhenDeleteFails()
    {
        var command = new DeleteOrganizerCommand(1);
        var result = Result.Failure(Error.NotFound("Organizer.NotFound", "Organizer not found"));

        _mockSender.Setup(s => s.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.DeleteAsync(1);

        Assert.IsType<NotFoundObjectResult>(actionResult);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenOrganizerExists()
    {
        var query = new GetOrganizerByIdQuery(1);
        var organizer = new OrganizerDTO { Id = 1, Name = "Organizer 1" };
        var result = Result<OrganizerDTO>.Success(organizer);

        _mockSender.Setup(s => s.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var actionResult = await _controller.GetOrganizerByIdAsync(1);

        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        var returnValue = Assert.IsType<OrganizerDTO>(okResult.Value);
        returnValue.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithList()
    {
        var queryParameters = new QueryParameters();
        var query = new GetAllOrganizersQuery(queryParameters);
        var organizers = new List<OrganizerDTO> { new() { Id = 1, Name = "Organizer 1" } };
        var pagedResult = new PagedResult<OrganizerDTO>(organizers, 1, 10, 1);

        _mockSender.Setup(s => s.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync(pagedResult);

        var actionResult = await _controller.GetAllOrganizersAsync(queryParameters);

        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        var returnValue = Assert.IsType<List<OrganizerDTO>>(okResult.Value);
        returnValue.Should().HaveCount(1);
    }
}
