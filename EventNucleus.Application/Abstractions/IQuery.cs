using MediatR;

namespace EventNucleus.Application.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}

