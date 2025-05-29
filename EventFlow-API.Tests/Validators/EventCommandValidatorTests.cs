using EventFlow_API.Commands;
using EventFlow_API.Validators;
using FluentValidation.TestHelper;

namespace EventFlow_API.Tests.Validators;

public class EventCommandValidatorTests
{
    private readonly EventCommandValidator _validator;

    public EventCommandValidatorTests()
    {
        _validator = new EventCommandValidator();
    }

    [Fact]
    public void Should_HaveError_When_DescriptionTooLong()
    {
        var model = new EventCommand { Title = "Valid Title", Description = new string('a', 8001), Date = DateTime.Now.AddDays(1), Location = "Valid", OrganizerId = 1 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(e => e.Description);
    }

    [Fact]
    public void Should_HaveError_When_DateInPast()
    {
        var model = new EventCommand { Title = "Valid Title", Description = "Valid", Date = DateTime.Now.AddDays(-1), Location = "Valid", OrganizerId = 1 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(e => e.Date);
    }

    [Fact]
    public void Should_HaveError_When_LocationEmpty()
    {
        var model = new EventCommand { Title = "Valid Title", Description = "Valid", Date = DateTime.Now.AddDays(1), Location = "", OrganizerId = 1 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(e => e.Location);
    }

    [Fact]
    public void Should_HaveError_When_OrganizerIdInvalid()
    {
        var model = new EventCommand { Title = "Valid Title", Description = "Valid", Date = DateTime.Now.AddDays(1), Location = "Valid", OrganizerId = 0 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(e => e.OrganizerId);
    }

    [Fact]
    public void Should_PassValidation_When_ValidData()
    {
        var model = new EventCommand { Title = "Valid Title", Description = "Valid", Date = DateTime.Now.AddDays(1), Location = "Valid Location", OrganizerId = 1 };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }

}
