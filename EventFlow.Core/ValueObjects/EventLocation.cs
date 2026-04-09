namespace EventFlow.Core.ValueObjects;

public sealed class EventLocation : ValueObject
{
    public string Address { get; }
    public string? City { get; }
    public string? State { get; }
    public string? ZipCode { get; }

    private EventLocation(string address, string? city, string? state, string? zipCode)
    {
        Address = address;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    public static EventLocation Create(string address, string? city = null, string? state = null, string? zipCode = null)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address cannot be empty", nameof(address));

        address = address.Trim();

        if (address.Length < 5)
            throw new ArgumentException("Address must be at least 5 characters", nameof(address));

        if (address.Length > 300)
            throw new ArgumentException("Address cannot exceed 300 characters", nameof(address));

        return new EventLocation(
            address,
            city?.Trim(),
            state?.Trim(),
            zipCode?.Trim());
    }

    public static bool TryCreate(string address, string? city, string? state, string? zipCode, out EventLocation? result)
    {
        try
        {
            result = Create(address, city, state, zipCode);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public string FullAddress
    {
        get
        {
            var parts = new List<string> { Address };
            if (!string.IsNullOrEmpty(City)) parts.Add(City);
            if (!string.IsNullOrEmpty(State)) parts.Add(State);
            if (!string.IsNullOrEmpty(ZipCode)) parts.Add(ZipCode);
            return string.Join(", ", parts);
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Address.ToLowerInvariant();
        yield return City?.ToLowerInvariant() ?? string.Empty;
        yield return State?.ToLowerInvariant() ?? string.Empty;
        yield return ZipCode?.ToLowerInvariant() ?? string.Empty;
    }

    public override string ToString() => FullAddress;

    public static implicit operator string(EventLocation location) => location.FullAddress;
}
