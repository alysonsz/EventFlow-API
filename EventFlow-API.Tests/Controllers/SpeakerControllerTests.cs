using Microsoft.AspNetCore.Http;

namespace EventFlow_API.Tests.Controllers;

public class SpeakerControllerTests
{
    private readonly SpeakerController _controller;
    private readonly Mock<ISpeakerService> _mockService;

    public SpeakerControllerTests()
    {
        _mockService = new Mock<ISpeakerService>();
        _controller = new SpeakerController(_mockService.Object);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public async Task PostAsync_ReturnsOk_WhenSpeakerCreated()
    {
        var command = new SpeakerCommand { Name = "Test", Email = "test@example.com", Biography = "Bio" };
        var speaker = new Speaker { Id = 1, Name = "Test", Email = "test@example.com", Biography = "Bio" };

        _mockService.Setup(s => s.CreateAsync(command)).ReturnsAsync(speaker);

        var result = await _controller.PostAsync(command);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<Speaker>(okResult.Value);
        returnValue.Id.Should().Be(1);
    }

    [Fact]
    public async Task PostAsync_ReturnsBadRequest_WhenCreationFails()
    {
        var command = new SpeakerCommand { Name = "Test", Email = "test@example.com", Biography = "Bio" };

        _mockService.Setup(s => s.CreateAsync(command)).ReturnsAsync((Speaker)null!);

        var result = await _controller.PostAsync(command);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsOk_WhenUpdated()
    {
        var command = new SpeakerCommand { Name = "Updated", Email = "updated@example.com", Biography = "Bio" };
        var updated = new SpeakerDTO { Id = 1, Name = "Updated" };

        _mockService.Setup(s => s.UpdateAsync(1, command)).ReturnsAsync(updated);

        var result = await _controller.UpdateAsync(1, command);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<SpeakerDTO>(okResult.Value);
        returnValue.Id.Should().Be(1);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNotFound_WhenUpdateFails()
    {
        var command = new SpeakerCommand { Name = "Updated", Email = "updated@example.com", Biography = "Bio" };

        _mockService.Setup(s => s.UpdateAsync(1, command)).ReturnsAsync((SpeakerDTO)null!);

        var result = await _controller.UpdateAsync(1, command);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsOk_WhenDeleted()
    {
        _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _controller.DeleteAsync(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(1, okResult.Value);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNotFound_WhenDeleteFails()
    {
        _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

        var result = await _controller.DeleteAsync(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task RegisterToEventAsync_ReturnsOk_WhenSuccess()
    {
        _mockService.Setup(s => s.RegisterToEventAsync(1, 1)).ReturnsAsync(true);

        var result = await _controller.RegisterToEventAsync(1, 1);

        var okResult = Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task RegisterToEventAsync_ReturnsNotFound_WhenFailure()
    {
        _mockService.Setup(s => s.RegisterToEventAsync(1, 1)).ReturnsAsync(false);

        var result = await _controller.RegisterToEventAsync(1, 1);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenSpeakerExists()
    {
        var speaker = new SpeakerDTO { Id = 1, Name = "Speaker 1" };
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(speaker);

        var result = await _controller.GetSpeakerByIdAsync(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<SpeakerDTO>(okResult.Value);
        returnValue.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithList()
    {
        var queryParameters = new QueryParameters();
        var speakers = new List<SpeakerDTO> { new() { Id = 1, Name = "Speaker 1" } };
        var pagedResult = new PagedResult<SpeakerDTO>(speakers, 1, 10, 1);

        _mockService
            .Setup(s => s.GetAllPagedSpeakersAsync(It.IsAny<QueryParameters>()))
            .ReturnsAsync(pagedResult);

        var result = await _controller.GetAllSpeakersAsync(queryParameters);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<SpeakerDTO>>(okResult.Value);
        returnValue.Should().HaveCount(1);
    }
}
