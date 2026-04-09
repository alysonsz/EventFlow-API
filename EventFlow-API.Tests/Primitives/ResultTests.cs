using EventFlow.Core.Primitives;

namespace EventFlow.Tests.Primitives;

public class ResultTests
{
    [Fact]
    public void Success_ReturnsIsSuccessTrue()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }

    [Fact]
    public void Failure_ReturnsIsFailureTrue()
    {
        var result = Result.Failure(Error.NotFound("Test", "Not found"));

        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Success_Generic_ReturnsValue()
    {
        var result = Result<int>.Success(42);

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Failure_Generic_ThrowsOnValueAccess()
    {
        var result = Result<int>.Failure(Error.NotFound("Test", "Not found"));

        Assert.True(result.IsFailure);
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    [Fact]
    public void ImplicitConversion_FromError_ReturnsFailure()
    {
        Error error = Error.Validation("Test", "Invalid");

        Result result = error;

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
    }

    [Fact]
    public void ImplicitConversion_FromValue_ReturnsSuccess()
    {
        Result<int> result = 42;

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void ImplicitConversion_NullValue_ReturnsFailure()
    {
        Result<string> result = (string?)null;

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Failure, result.Error.Type);
    }

    [Fact]
    public void Constructor_SuccessWithError_ThrowsException()
    {
        Assert.Throws<InvalidOperationException>(() => Result.Failure(Error.None));
    }

    [Fact]
    public void Map_OnSuccess_ReturnsTransformedValue()
    {
        var result = Result<int>.Success(5);

        var mapped = result.Map(v => v * 2, e => 0);

        Assert.Equal(10, mapped);
    }

    [Fact]
    public void Map_OnFailure_ReturnsErrorHandlerResult()
    {
        var result = Result<int>.Failure(Error.NotFound("Test", "Not found"));

        var mapped = result.Map(v => v * 2, e => -1);

        Assert.Equal(-1, mapped);
    }
}
