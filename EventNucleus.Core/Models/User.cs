namespace EventNucleus.Core.Models;

public class User : Entity<int>
{
    public string Username { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }

    private User() { }

    public static User Create(string username, string email, string passwordHash)
    {
        CheckRule(new UserUsernameCannotBeEmptyRule(username));
        CheckRule(new UserPasswordHashCannotBeEmptyRule(passwordHash));

        var user = new User
        {
            Username = username,
            Email = Email.Create(email),
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };

        return user;
    }

    public void UpdateEmail(string email)
    {
        Email = Email.Create(email);
    }

    public void UpdatePassword(string passwordHash)
    {
        CheckRule(new UserPasswordHashCannotBeEmptyRule(passwordHash));
        PasswordHash = passwordHash;
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }
}

public class UserUsernameCannotBeEmptyRule : IBusinessRule
{
    private readonly string _username;

    public UserUsernameCannotBeEmptyRule(string username)
    {
        _username = username;
    }

    public bool IsBroken() => string.IsNullOrWhiteSpace(_username);
    public string Message => "Username cannot be empty";
}

public class UserPasswordHashCannotBeEmptyRule : IBusinessRule
{
    private readonly string _passwordHash;

    public UserPasswordHashCannotBeEmptyRule(string passwordHash)
    {
        _passwordHash = passwordHash;
    }

    public bool IsBroken() => string.IsNullOrWhiteSpace(_passwordHash);
    public string Message => "Password hash cannot be empty";
}

