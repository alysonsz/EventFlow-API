using MediatR;

namespace EventFlow.Application.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
