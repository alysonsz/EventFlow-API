namespace EventFlow.Core.ValueObjects;

public sealed class EventTitle : ValueObject
{
    public string Value { get; }

    private EventTitle(string value)
    {
        Value = value;
    }

    public static EventTitle Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Event title cannot be empty", nameof(title));

        title = title.Trim();

        if (title.Length < 3)
            throw new ArgumentException("Event title must be at least 3 characters", nameof(title));

        if (title.Length > 200)
            throw new ArgumentException("Event title cannot exceed 200 characters", nameof(title));

        return new EventTitle(title);
    }

    public static bool TryCreate(string title, out EventTitle? result)
    {
        try
        {
            result = Create(title);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(EventTitle title) => title.Value;
}
