namespace EventFlow_API.Tests.ValueObjects;

public class EventTitleTests
{
    [Fact]
    public void Create_ValidTitle_ReturnsEventTitle()
    {
        var result = EventTitle.Create("Tech Conference 2024");

        result.Should().NotBeNull();
        result.Value.Should().Be("Tech Conference 2024");
    }

    [Fact]
    public void Create_ShortTitle_ThrowsArgumentException()
    {
        Action act = () => EventTitle.Create("AB");

        act.Should().Throw<ArgumentException>()
           .WithMessage("*at least 3 characters*");
    }

    [Fact]
    public void Create_LongTitle_ThrowsArgumentException()
    {
        var longTitle = new string('A', 201);
        Action act = () => EventTitle.Create(longTitle);

        act.Should().Throw<ArgumentException>()
           .WithMessage("*cannot exceed 200 characters*");
    }

    [Fact]
    public void Create_EmptyTitle_ThrowsArgumentException()
    {
        Action act = () => EventTitle.Create("");

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_TrimmedTitle_ReturnsTrimmedValue()
    {
        var result = EventTitle.Create("  Tech Conference 2024  ");

        result.Value.Should().Be("Tech Conference 2024");
    }

    [Fact]
    public void TryCreate_ValidTitle_ReturnsTrue()
    {
        var success = EventTitle.TryCreate("Valid Title", out var result);

        success.Should().BeTrue();
        result.Should().NotBeNull();
    }

    [Fact]
    public void Equals_SameValue_ReturnsTrue()
    {
        var title1 = EventTitle.Create("Conference");
        var title2 = EventTitle.Create("Conference");

        title1.Should().Be(title2);
    }

    [Fact]
    public void ImplicitConversion_ToString_ReturnsValue()
    {
        var title = EventTitle.Create("Conference");

        string result = title;

        result.Should().Be("Conference");
    }
}
