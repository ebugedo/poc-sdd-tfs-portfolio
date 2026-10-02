using MediatR;

namespace Portfolio.Application.Commands;

public abstract class CommandBase<TResponse> : IRequest<TResponse>;