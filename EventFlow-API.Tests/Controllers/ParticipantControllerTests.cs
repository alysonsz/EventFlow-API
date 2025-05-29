using EventFlow_API.Commands;
using EventFlow_API.Controllers;
using EventFlow_API.Models;
using EventFlow_API.Models.DTOs;
using EventFlow_API.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EventFlow_API.Tests.Controllers;

public class ParticipantControllerTests
{
    private readonly ParticipantController _controller;
    private readonly Mock<IParticipantService> _mockService;

    public ParticipantControllerTests()
    {
        _mockService = new Mock<IParticipantService>();
        _controller = new ParticipantController(_mockService.Object);
    }

    [Fact]
    public async Task PostAsync_ReturnsOk_WhenParticipantCreated()
    {
        var command = new ParticipantCommand { Name = "Test", Email = "test@example.com" };
        var participant = new Participant { Id = 1, Name = "Test", Email = "test@example.com" };

        _mockService.Setup(s => s.CreateAsync(command)).ReturnsAsync(participant);

        var result = await _controller.PostAsync(command);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<Participant>(okResult.Value);
        returnValue.Id.Should().Be(1);
    }

    [Fact]
    public async Task PostAsync_ReturnsBadRequest_WhenCreationFails()
    {
        var command = new ParticipantCommand { Name = "Test", Email = "test@example.com" };

        _mockService.Setup(s => s.CreateAsync(command)).ReturnsAsync((Participant)null!);

        var result = await _controller.PostAsync(command);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsOk_WhenUpdated()
    {
        var command = new ParticipantCommand { Name = "Updated", Email = "updated@example.com" };
        var updated = new ParticipantDTO { Id = 1, Name = "Updated" };

        _mockService.Setup(s => s.UpdateAsync(1, command)).ReturnsAsync(updated);

        var result = await _controller.UpdateAsync(1, command);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ParticipantDTO>(okResult.Value);
        returnValue.Id.Should().Be(1);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNotFound_WhenUpdateFails()
    {
        var command = new ParticipantCommand { Name = "Updated", Email = "updated@example.com" };

        _mockService.Setup(s => s.UpdateAsync(1, command)).ReturnsAsync((ParticipantDTO)null!);

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
    public async Task GetById_ReturnsOk_WhenParticipantExists()
    {
        var participant = new ParticipantDTO { Id = 1, Name = "Participant 1" };
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(participant);

        var result = await _controller.GetParticipantByIdAsync(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ParticipantDTO>(okResult.Value);
        returnValue.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithList()
    {
        var eventId = 1;
        var participants = new List<ParticipantDTO> { new ParticipantDTO { Id = 1, Name = "Participant 1" } };
        _mockService.Setup(s => s.GetAllAsync(eventId)).ReturnsAsync(participants);

        var result = await _controller.GetAllParticipantsAsync(eventId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<ParticipantDTO>>(okResult.Value);
        returnValue.Should().HaveCount(1);
    }
}
