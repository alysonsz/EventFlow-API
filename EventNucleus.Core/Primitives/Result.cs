using System.Diagnostics.CodeAnalysis;

namespace EventNucleus.Core.Primitives;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
    
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("Cannot create success result with error");
            
        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("Cannot create failure result with empty error");
            
        IsSuccess = isSuccess;
        Error = error;
    }
    
    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    
    public static implicit operator Result(Error error) => Failure(error);
}

public class Result<T> : Result
{
    private readonly T? _value;
    
    [NotNullIfNotNull(nameof(_value))]
    public T? Value => IsSuccess ? _value : throw new InvalidOperationException("Cannot access value of failed result");
    
    protected Result(T? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }
    
    public static Result<T> Success(T value) => new(value, true, Error.None);
    public new static Result<T> Failure(Error error) => new(default, false, error);
    
    public static implicit operator Result<T>(T? value) => value is not null ? Success(value) : Failure(Error.NullValue());
    public static implicit operator Result<T>(Error error) => Failure(error);
    
    public TResult Map<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(_value!) : onFailure(Error);
    }
    
    public async Task<TResult> MapAsync<TResult>(Func<T, Task<TResult>> onSuccess, Func<Error, Task<TResult>> onFailure)
    {
        return IsSuccess ? await onSuccess(_value!) : await onFailure(Error);
    }
}

