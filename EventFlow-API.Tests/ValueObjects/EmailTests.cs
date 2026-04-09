namespace EventFlow_API.Tests.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Create_ValidEmail_ReturnsEmail()
    {
        var result = Email.Create("test@example.com");

        result.Should().NotBeNull();
        result.Value.Should().Be("test@example.com");
    }

    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name@domain.co")]
    [InlineData("user+tag@example.org")]
    public void Create_ValidEmails_AcceptAll(string email)
    {
        var result = Email.Create(email);

        result.Value.Should().Be(email.ToLowerInvariant());
    }

    [Fact]
    public void Create_EmptyEmail_ThrowsArgumentException()
    {
        Action act = () => Email.Create("");

        act.Should().Throw<ArgumentException>()
           .WithMessage("*cannot be empty*");
    }

    [Fact]
    public void Create_InvalidEmail_ThrowsArgumentException()
    {
        Action act = () => Email.Create("invalid-email");

        act.Should().Throw<ArgumentException>()
           .WithMessage("*Invalid email format*");
    }

    [Fact]
    public void Create_NullEmail_ThrowsArgumentException()
    {
        Action act = () => Email.Create(null!);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void TryCreate_ValidEmail_ReturnsTrue()
    {
        var success = Email.TryCreate("test@example.com", out var result);

        success.Should().BeTrue();
        result.Should().NotBeNull();
        result!.Value.Should().Be("test@example.com");
    }

    [Fact]
    public void TryCreate_InvalidEmail_ReturnsFalse()
    {
        var success = Email.TryCreate("invalid", out var result);

        success.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void Equals_SameValue_ReturnsTrue()
    {
        var email1 = Email.Create("test@example.com");
        var email2 = Email.Create("test@example.com");

        email1.Should().Be(email2);
        (email1 == email2).Should().BeTrue();
    }

    [Fact]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        var email1 = Email.Create("test1@example.com");
        var email2 = Email.Create("test2@example.com");

        email1.Should().NotBe(email2);
        (email1 != email2).Should().BeTrue();
    }

    [Fact]
    public void Equals_CaseInsensitive_ReturnsTrue()
    {
        var email1 = Email.Create("Test@Example.COM");
        var email2 = Email.Create("test@example.com");

        email1.Should().Be(email2);
    }

    [Fact]
    public void ImplicitConversion_ToString_ReturnsValue()
    {
        var email = Email.Create("test@example.com");

        string result = email;

        result.Should().Be("test@example.com");
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        var email = Email.Create("test@example.com");

        email.ToString().Should().Be("test@example.com");
    }
}
