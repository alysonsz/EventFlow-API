using AutoMapper;
using EventFlow.Core.Repository.Interfaces;

namespace EventFlow_API.Tests.Services;

public class OrganizerServiceTests
{
    private readonly OrganizerService _service;
    private readonly Mock<IOrganizerRepository> _mockOrganizerRepository;
    private readonly Mock<IEventRepository> _mockEventRepository;
    private readonly Mock<IMapper> _mockMapper;

    public OrganizerServiceTests()
    {
        _mockOrganizerRepository = new Mock<IOrganizerRepository>();
        _mockEventRepository = new Mock<IEventRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new OrganizerService(_mockOrganizerRepository.Object, _mockEventRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnOrganizer_WhenSuccessful()
    {
        var command = new OrganizerCommand { Name = "Test", Email = "test@example.com" };
        var organizer = new Organizer { Id = 1, Name = command.Name, Email = command.Email };

        _mockOrganizerRepository.Setup(r => r.PostAsync(It.IsAny<Organizer>())).ReturnsAsync(organizer);

        var result = await _service.CreateAsync(command);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedOrganizer_WhenSuccessful()
    {
        var command = new OrganizerCommand { Name = "Updated", Email = "updated@example.com" };
        var existing = new Organizer { Id = 1, Name = "Old", Email = "old@example.com" };
        var updated = new Organizer { Id = 1, Name = command.Name, Email = command.Email };
        var updatedDTO = new OrganizerDTO { Id = 1, Name = command.Name };

        _mockOrganizerRepository.Setup(r => r.GetOrganizerByIdAsync(1)).ReturnsAsync(existing);
        _mockOrganizerRepository.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(updated);
        _mockMapper.Setup(m => m.Map<OrganizerDTO>(updated)).Returns(updatedDTO);

        var result = await _service.UpdateAsync(1, command);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenOrganizerNotFound()
    {
        _mockOrganizerRepository.Setup(r => r.GetOrganizerByIdAsync(1)).ReturnsAsync((Organizer)null!);

        var result = await _service.UpdateAsync(1, new OrganizerCommand { Name = "Name", Email = "Email" });

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenUpdateFails()
    {
        var command = new OrganizerCommand { Name = "Test", Email = "Email" };
        var existing = new Organizer { Id = 1 };

        _mockOrganizerRepository.Setup(r => r.GetOrganizerByIdAsync(1)).ReturnsAsync(existing);
        _mockOrganizerRepository.Setup(r => r.UpdateAsync(existing)).ReturnsAsync((Organizer)null!);

        var result = await _service.UpdateAsync(1, command);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenSuccessful()
    {
        _mockOrganizerRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(1);

        var result = await _service.DeleteAsync(1);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenUnsuccessful()
    {
        _mockOrganizerRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(0);

        var result = await _service.DeleteAsync(1);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task RegisterToEventAsync_ShouldReturnTrue_WhenSuccessful()
    {
        var organizer = new Organizer { Id = 1 };
        var evento = new Event { Id = 1, OrganizerId = 0 };

        _mockOrganizerRepository.Setup(r => r.GetOrganizerByIdAsync(1)).ReturnsAsync(organizer);
        _mockEventRepository.Setup(r => r.GetEventByIdAsync(1)).ReturnsAsync(evento);
        _mockEventRepository.Setup(r => r.UpdateAsync(evento)).ReturnsAsync(evento);

        var result = await _service.RegisterToEventAsync(1, 1);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task RegisterToEventAsync_ShouldReturnFalse_WhenOrganizerOrEventNotFound()
    {
        _mockOrganizerRepository.Setup(r => r.GetOrganizerByIdAsync(1)).ReturnsAsync((Organizer)null!);
        _mockEventRepository.Setup(r => r.GetEventByIdAsync(1)).ReturnsAsync(new Event { Id = 1 });

        var result1 = await _service.RegisterToEventAsync(1, 1);
        result1.Should().BeFalse();

        _mockOrganizerRepository.Setup(r => r.GetOrganizerByIdAsync(1)).ReturnsAsync(new Organizer { Id = 1 });
        _mockEventRepository.Setup(r => r.GetEventByIdAsync(1)).ReturnsAsync((Event)null!);

        var result2 = await _service.RegisterToEventAsync(1, 1);
        result2.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnOrganizer()
    {
        var organizer = new Organizer { Id = 1, Name = "Organizer 1" };
        var organizerDTO = new OrganizerDTO { Id = 1, Name = "Organizer 1" };

        _mockOrganizerRepository.Setup(r => r.GetOrganizerByIdAsync(1)).ReturnsAsync(organizer);
        _mockMapper.Setup(m => m.Map<OrganizerDTO>(organizer)).Returns(organizerDTO);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedResultOrganizers()
    {
        var queryParameters = new QueryParameters
        {
            PageNumber = 1,
            PageSize = 10,
            Filter = null,
            SortBy = null
        };

        var organizers = new List<Organizer>
    {
        new Organizer { Id = 1, Name = "Organizer 1" }
    };

        var pagedResult = new PagedResult<Organizer>(
            organizers,
            queryParameters.PageNumber,
            queryParameters.PageSize,
            totalCount: 1
        );

        var organizerDTOs = new List<OrganizerDTO>
    {
        new OrganizerDTO { Id = 1, Name = "Organizer 1" }
    };

        _mockOrganizerRepository
            .Setup(r => r.GetAllOrganizersAsync(queryParameters))
            .ReturnsAsync(pagedResult);

        _mockMapper
            .Setup(m => m.Map<List<OrganizerDTO>>(organizers))
            .Returns(organizerDTOs);

        var result = await _service.GetAllOrganizersAsync(queryParameters);

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Items.First().Id.Should().Be(1);
        result.TotalCount.Should().Be(1);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
    }
}
