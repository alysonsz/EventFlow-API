namespace EventNucleus.Core.ValueObjects;

public sealed class PersonName : ValueObject
{
    public string FirstName { get; }
    public string LastName { get; }

    private PersonName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static PersonName Create(string firstName, string? lastName = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));

        firstName = firstName.Trim();

        if (firstName.Length < 2)
            throw new ArgumentException("First name must be at least 2 characters", nameof(firstName));

        if (firstName.Length > 100)
            throw new ArgumentException("First name cannot exceed 100 characters", nameof(firstName));

        lastName = lastName?.Trim() ?? string.Empty;

        if (lastName.Length > 100)
            throw new ArgumentException("Last name cannot exceed 100 characters", nameof(lastName));

        return new PersonName(firstName, lastName);
    }

    public static bool TryCreate(string firstName, string? lastName, out PersonName? result)
    {
        try
        {
            result = Create(firstName, lastName);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public string FullName => string.IsNullOrEmpty(LastName)
        ? FirstName
        : $"{FirstName} {LastName}";

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName.ToLowerInvariant();
        yield return LastName.ToLowerInvariant();
    }

    public override string ToString() => FullName;

    public static implicit operator string(PersonName name) => name.FullName;
}

