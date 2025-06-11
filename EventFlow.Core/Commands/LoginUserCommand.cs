namespace EventFlow.Core.Commands;

public class LoginUserCommand
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
