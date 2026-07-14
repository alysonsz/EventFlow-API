namespace EventNucleus.Core.Primitives;

public interface IBusinessRule
{
    bool IsBroken();
    string Message { get; }
}

