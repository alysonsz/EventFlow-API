using EventFlow_API.Commands;
using EventFlow_API.Controllers;
using EventFlow_API.Models;
using EventFlow_API.Models.DTOs;
using EventFlow_API.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EventFlow_API.Tests.Controllers;

public class OrganizerControllerTests
{
    private readonly OrganizerController _controller;
    private readonly Mock<IOrganizerService> _mockService;

    public OrganizerControllerTests()
    {
        _mockService = new Mock<IOrganizerService>();
        _controller = new OrganizerController(_mockService.Object);
    }

    [Fact]
    public async Task PostAsync_ReturnsOk_WhenOrganizerCreated()
    {
        var command = new OrganizerCommand { Name = "Test", Email = "test@example.com" };
        var organizer = new Organizer { Id = 1, Name = "Test", Email = "test@example.com" };

        _mockService.Setup(s => s.CreateAsync(command)).ReturnsAsync(organizer);

        var result = await _controller.PostAsync(command);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<Organizer>(okResult.Value);
        returnValue.Id.Should().Be(1);
    }

    [Fact]
    public async Task PostAsync_ReturnsBadRequest_WhenCreationFails()
    {
        var command = new OrganizerCommand { Name = "Test", Email = "test@example.com" };

        _mockService.Setup(s => s.CreateAsync(command)).ReturnsAsync((Organizer)null!);

        var result = await _controller.PostAsync(command);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsOk_WhenUpdated()
    {
        var command = new OrganizerCommand { Name = "Updated", Email = "updated@example.com" };
        var updated = new OrganizerDTO { Id = 1, Name = "Updated" };

        _mockService.Setup(s => s.UpdateAsync(1, command)).ReturnsAsync(updated);

        var result = await _controller.UpdateAsync(1, command);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<OrganizerDTO>(okResult.Value);
        returnValue.Id.Should().Be(1);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNotFound_WhenUpdateFails()
    {
        var command = new OrganizerCommand { Name = "Updated", Email = "updated@example.com" };

        _mockService.Setup(s => s.UpdateAsync(1, command)).ReturnsAsync((OrganizerDTO)null!);

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
    public async Task RegisterParticipantAsync_ReturnsOk_WhenSuccess()
    {
        _mockService.Setup(s => s.RegisterToEventAsync(1, 1)).ReturnsAsync(true);

        var result = await _controller.RegisterParticipantAsync(1, 1);

        var okResult = Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task RegisterParticipantAsync_ReturnsNotFound_WhenFailure()
    {
        _mockService.Setup(s => s.RegisterToEventAsync(1, 1)).ReturnsAsync(false);

        var result = await _controller.RegisterParticipantAsync(1, 1);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenOrganizerExists()
    {
        var organizer = new OrganizerDTO { Id = 1, Name = "Organizer 1" };
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(organizer);

        var result = await _controller.GetOrganizerByIdAsync(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<OrganizerDTO>(okResult.Value);
        returnValue.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithList()
    {
        var organizers = new List<OrganizerDTO> { new OrganizerDTO { Id = 1, Name = "Organizer 1" } };
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(organizers);

        var result = await _controller.GetAllOrganizersAsync();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<OrganizerDTO>>(okResult.Value);
        returnValue.Should().HaveCount(1);
    }
}
