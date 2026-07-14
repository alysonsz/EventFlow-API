namespace EventNucleus.Core.Primitives;

public interface IDomainEvent
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
}

