using EventFlow_API.Commands;
using EventFlow_API.Validators;
using FluentValidation.TestHelper;

namespace EventFlow_API.Tests.Validators;

public class ParticipantCommandValidatorTests
{
    private readonly ParticipantCommandValidator _validator;

    public ParticipantCommandValidatorTests()
    {
        _validator = new ParticipantCommandValidator();
    }

    [Fact]
    public void Should_HaveError_When_NameIsEmpty()
    {
        var model = new ParticipantCommand { Name = "", Email = "valid@example.com" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(p => p.Name);
    }

    [Fact]
    public void Should_HaveError_When_EmailIsEmpty()
    {
        var model = new ParticipantCommand { Name = "Valid Name", Email = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(p => p.Email);
    }

    [Fact]
    public void Should_HaveError_When_EmailInvalid()
    {
        var model = new ParticipantCommand { Name = "Valid Name", Email = "invalid-email" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(p => p.Email);
    }

    [Fact]
    public void Should_PassValidation_When_ValidData()
    {
        var model = new ParticipantCommand { Name = "Valid Name", Email = "valid@example.com" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
