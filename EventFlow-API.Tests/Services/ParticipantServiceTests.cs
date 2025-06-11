using AutoMapper;
using EventFlow.Core.Repository.Interfaces;

namespace EventFlow_API.Tests.Services;

public class ParticipantServiceTests
{
    private readonly ParticipantService _service;
    private readonly Mock<IParticipantRepository> _mockParticipantRepository;
    private readonly Mock<IEventRepository> _mockEventRepository;
    private readonly Mock<IMapper> _mockMapper;

    public ParticipantServiceTests()
    {
        _mockParticipantRepository = new Mock<IParticipantRepository>();
        _mockEventRepository = new Mock<IEventRepository>();
        _mockMapper = new Mock<IMapper>();

        var options = new DbContextOptionsBuilder<EventFlowContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _service = new ParticipantService(_mockParticipantRepository.Object, _mockEventRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnParticipant_WhenSuccessful()
    {
        var command = new ParticipantCommand { Name = "Test", Email = "test@example.com" };
        var participant = new Participant { Id = 1, Name = command.Name, Email = command.Email };

        _mockParticipantRepository.Setup(r => r.PostAsync(It.IsAny<Participant>())).ReturnsAsync(participant);

        var result = await _service.CreateAsync(command);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedParticipant_WhenSuccessful()
    {
        var command = new ParticipantCommand { Name = "Updated", Email = "updated@example.com" };
        var existing = new Participant { Id = 1, Name = "Old", Email = "old@example.com" };
        var updated = new Participant { Id = 1, Name = command.Name, Email = command.Email };
        var updatedDTO = new ParticipantDTO { Id = 1, Name = command.Name };

        _mockParticipantRepository.Setup(r => r.GetParticipantByIdAsync(1)).ReturnsAsync(existing);
        _mockParticipantRepository.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(updated);
        _mockMapper.Setup(m => m.Map<ParticipantDTO>(updated)).Returns(updatedDTO);

        var result = await _service.UpdateAsync(1, command);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenParticipantNotFound()
    {
        _mockParticipantRepository.Setup(r => r.GetParticipantByIdAsync(1)).ReturnsAsync((Participant)null!);

        var result = await _service.UpdateAsync(1, new ParticipantCommand { Name = "Name", Email = "Email" });

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenUpdateFails()
    {
        var command = new ParticipantCommand { Name = "Test", Email = "Email" };
        var existing = new Participant { Id = 1 };

        _mockParticipantRepository.Setup(r => r.GetParticipantByIdAsync(1)).ReturnsAsync(existing);
        _mockParticipantRepository.Setup(r => r.UpdateAsync(existing)).ReturnsAsync((Participant)null!);

        var result = await _service.UpdateAsync(1, command);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenSuccessful()
    {
        _mockParticipantRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(1);

        var result = await _service.DeleteAsync(1);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenUnsuccessful()
    {
        _mockParticipantRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(0);

        var result = await _service.DeleteAsync(1);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task RegisterToEventAsync_ShouldReturnTrue_WhenSuccessful()
    {
        var participant = new Participant { Id = 1, Events = new List<Event>() };
        var evento = new Event { Id = 1 };

        _mockParticipantRepository.Setup(r => r.GetParticipantByIdAsync(1)).ReturnsAsync(participant);
        _mockEventRepository.Setup(r => r.GetEventByIdAsync(1)).ReturnsAsync(evento);
        _mockParticipantRepository.Setup(r => r.UpdateAsync(participant)).ReturnsAsync(participant);

        var result = await _service.RegisterToEventAsync(1, 1);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task RegisterToEventAsync_ShouldReturnFalse_WhenParticipantOrEventNotFound()
    {
        _mockParticipantRepository.Setup(r => r.GetParticipantByIdAsync(1)).ReturnsAsync((Participant)null!);
        _mockEventRepository.Setup(r => r.GetEventByIdAsync(1)).ReturnsAsync(new Event { Id = 1 });

        var result1 = await _service.RegisterToEventAsync(1, 1);
        result1.Should().BeFalse();

        _mockParticipantRepository.Setup(r => r.GetParticipantByIdAsync(1)).ReturnsAsync(new Participant { Id = 1, Events = new List<Event>() });
        _mockEventRepository.Setup(r => r.GetEventByIdAsync(1)).ReturnsAsync((Event)null!);

        var result2 = await _service.RegisterToEventAsync(1, 1);
        result2.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnParticipant()
    {
        var participant = new Participant { Id = 1, Name = "Participant 1" };
        var participantDTO = new ParticipantDTO { Id = 1, Name = "Participant 1" };

        _mockParticipantRepository.Setup(r => r.GetParticipantByIdAsync(1)).ReturnsAsync(participant);
        _mockMapper.Setup(m => m.Map<ParticipantDTO>(participant)).Returns(participantDTO);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnParticipants()
    {
        var participants = new List<Participant> { new Participant { Id = 1, Name = "Participant 1" } };
        var participantDTOs = new List<ParticipantDTO> { new ParticipantDTO { Id = 1, Name = "Participant 1" } };

        _mockParticipantRepository.Setup(r => r.GetAllParticipantsByEventIdAsync(1)).ReturnsAsync(participants);
        _mockMapper.Setup(m => m.Map<List<ParticipantDTO>>(participants)).Returns(participantDTOs);

        var result = await _service.GetAllAsync(1);

        result.Should().NotBeNull().And.HaveCount(1);
        result.First().Id.Should().Be(1);
    }
}
