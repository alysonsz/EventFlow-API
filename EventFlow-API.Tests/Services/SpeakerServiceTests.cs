using AutoMapper;
using EventFlow.Core.Repository.Interfaces;
using EventFlow.Infrastructure.Data;

namespace EventFlow_API.Tests.Services;

public class SpeakerServiceTests
{
    private readonly SpeakerService _service;
    private readonly Mock<ISpeakerRepository> _mockRepository;
    private readonly Mock<IEventRepository> _mockEventRepository;
    private readonly Mock<IMapper> _mockMapper;

    public SpeakerServiceTests()
    {
        _mockRepository = new Mock<ISpeakerRepository>();
        _mockEventRepository = new Mock<IEventRepository>();
        _mockMapper = new Mock<IMapper>();

        var options = new DbContextOptionsBuilder<EventFlowContext>()
        .UseInMemoryDatabase(databaseName: "TestDatabase")
        .Options;

        _service = new SpeakerService(_mockRepository.Object, _mockEventRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSpeaker_WhenSuccessful()
    {
        var command = new SpeakerCommand { Name = "Test", Email = "test@example.com", Biography = "Bio" };
        var speaker = new Speaker { Id = 1, Name = command.Name, Email = command.Email, Biography = command.Biography };

        _mockRepository.Setup(r => r.PostAsync(It.IsAny<Speaker>())).ReturnsAsync(speaker);

        var result = await _service.CreateAsync(command);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedSpeaker_WhenSuccessful()
    {
        var command = new SpeakerCommand { Name = "Updated", Email = "updated@example.com", Biography = "New Bio" };
        var existing = new Speaker { Id = 1, Name = "Old", Email = "old@example.com", Biography = "Old Bio" };
        var updated = new Speaker { Id = 1, Name = command.Name, Email = command.Email, Biography = command.Biography };
        var updatedDTO = new SpeakerDTO { Id = 1, Name = command.Name };

        _mockRepository.Setup(r => r.GetSpeakerByIdAsync(1)).ReturnsAsync(existing);
        _mockRepository.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(updated);
        _mockMapper.Setup(m => m.Map<SpeakerDTO>(updated)).Returns(updatedDTO);

        var result = await _service.UpdateAsync(1, command);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenSpeakerNotFound()
    {
        _mockRepository.Setup(r => r.GetSpeakerByIdAsync(1)).ReturnsAsync((Speaker)null!);

        var result = await _service.UpdateAsync(1, new SpeakerCommand { Name = "Name", Email = "Email", Biography = "Bio" });

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenUpdateFails()
    {
        var command = new SpeakerCommand { Name = "Test", Email = "Email", Biography = "Bio" };
        var existing = new Speaker { Id = 1 };

        _mockRepository.Setup(r => r.GetSpeakerByIdAsync(1)).ReturnsAsync(existing);
        _mockRepository.Setup(r => r.UpdateAsync(existing)).ReturnsAsync((Speaker)null!);

        var result = await _service.UpdateAsync(1, command);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenSuccessful()
    {
        _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(1);

        var result = await _service.DeleteAsync(1);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenUnsuccessful()
    {
        _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(0);

        var result = await _service.DeleteAsync(1);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task RegisterToEventAsync_ShouldReturnTrue_WhenSuccessful()
    {
        var speaker = new Speaker { Id = 1, SpeakerEvents = new List<SpeakerEvent>() };
        var evento = new Event { Id = 1 };

        _mockRepository.Setup(r => r.GetSpeakerByIdAsync(1)).ReturnsAsync(speaker);
        _mockEventRepository.Setup(r => r.GetEventByIdAsync(1)).ReturnsAsync(evento);

        var result = await _service.RegisterToEventAsync(1, 1);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task RegisterToEventAsync_ShouldReturnFalse_WhenSpeakerOrEventNotFound()
    {
        _mockRepository.Setup(r => r.GetSpeakerByIdAsync(1)).ReturnsAsync((Speaker)null!);
        _mockEventRepository.Setup(r => r.GetEventByIdAsync(1)).ReturnsAsync(new Event { Id = 1 });

        var result1 = await _service.RegisterToEventAsync(1, 1);
        result1.Should().BeFalse();

        _mockRepository.Setup(r => r.GetSpeakerByIdAsync(1)).ReturnsAsync(new Speaker { Id = 1, SpeakerEvents = new List<SpeakerEvent>() });
        _mockEventRepository.Setup(r => r.GetEventByIdAsync(1)).ReturnsAsync((Event)null!);

        var result2 = await _service.RegisterToEventAsync(1, 1);
        result2.Should().BeFalse();
    }

    [Fact]
    public async Task RegisterToEventAsync_ShouldReturnTrue_WhenAlreadyLinked()
    {
        var speaker = new Speaker { Id = 1, SpeakerEvents = new List<SpeakerEvent> { new SpeakerEvent { EventId = 1 } } };
        var evento = new Event { Id = 1 };

        _mockRepository.Setup(r => r.GetSpeakerByIdAsync(1)).ReturnsAsync(speaker);
        _mockEventRepository.Setup(r => r.GetEventByIdAsync(1)).ReturnsAsync(evento);

        var result = await _service.RegisterToEventAsync(1, 1);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSpeaker()
    {
        var speaker = new Speaker { Id = 1, Name = "Speaker 1" };
        var speakerDTO = new SpeakerDTO { Id = 1, Name = "Speaker 1" };

        _mockRepository.Setup(r => r.GetSpeakerByIdAsync(1)).ReturnsAsync(speaker);
        _mockMapper.Setup(m => m.Map<SpeakerDTO>(speaker)).Returns(speakerDTO);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedResultSpeakers()
    {
        var queryParameters = new QueryParameters
        {
            PageNumber = 1,
            PageSize = 10,
            Filter = null,
            SortBy = null
        };

        var speakers = new List<Speaker>
    {
        new Speaker { Id = 1, Name = "Speaker 1" }
    };

        var pagedResult = new PagedResult<Speaker>(
            speakers,
            queryParameters.PageNumber,
            queryParameters.PageSize,
            totalCount: 1
        );

        var speakerDTOs = new List<SpeakerDTO>
    {
        new SpeakerDTO { Id = 1, Name = "Speaker 1" }
    };

        _mockRepository
            .Setup(r => r.GetAllPagedSpeakersAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        _mockMapper
            .Setup(m => m.Map<List<SpeakerDTO>>(speakers))
            .Returns(speakerDTOs);

        var result = await _service.GetAllPagedSpeakersAsync(queryParameters);

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Items.First().Id.Should().Be(1);
        result.TotalCount.Should().Be(1);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
    }
}
