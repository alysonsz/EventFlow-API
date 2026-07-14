namespace EventNucleus.Core.Primitives;

public readonly record struct Error
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
    
    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }
    
    private Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }
    
    public static Error NotFound(string code, string message) => 
        new(code, message, ErrorType.NotFound);
    
    public static Error Validation(string code, string message) => 
        new(code, message, ErrorType.Validation);
    
    public static Error Conflict(string code, string message) => 
        new(code, message, ErrorType.Conflict);
    
    public static Error Failure(string code, string message) => 
        new(code, message, ErrorType.Failure);
    
    public static Error Unauthorized(string code, string message) => 
        new(code, message, ErrorType.Unauthorized);
    
    public static Error Forbidden(string code, string message) => 
        new(code, message, ErrorType.Forbidden);
    
    public static Error NullValue(string? code = null, string? message = null) => 
        new(code ?? "General.Null", message ?? "Value cannot be null", ErrorType.Failure);
}

public enum ErrorType
{
    None,
    NotFound,
    Validation,
    Conflict,
    Failure,
    Unauthorized,
    Forbidden
}

