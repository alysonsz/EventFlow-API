using FluentValidation.TestHelper;

namespace EventFlow_API.Tests.Validators;

public class SpeakerCommandValidatorTests
{
    private readonly SpeakerCommandValidator _validator;

    public SpeakerCommandValidatorTests()
    {
        _validator = new SpeakerCommandValidator();
    }

    [Fact]
    public void Should_HaveError_When_NameIsEmpty()
    {
        var model = new SpeakerCommand { Name = "", Email = "valid@example.com", Biography = "Valid Bio" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(s => s.Name);
    }

    [Fact]
    public void Should_HaveError_When_EmailInvalid()
    {
        var model = new SpeakerCommand { Name = "Valid Name", Email = "invalid-email", Biography = "Valid Bio" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(s => s.Email);
    }

    [Fact]
    public void Should_HaveError_When_BiographyEmpty()
    {
        var model = new SpeakerCommand { Name = "Valid Name", Email = "valid@example.com", Biography = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(s => s.Biography);
    }

    [Fact]
    public void Should_PassValidation_When_ValidData()
    {
        var model = new SpeakerCommand { Name = "Valid Name", Email = "valid@example.com", Biography = "Valid Biography", EventId = 1 };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}