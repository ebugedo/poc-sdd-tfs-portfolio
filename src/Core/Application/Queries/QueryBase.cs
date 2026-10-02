using MediatR;

namespace Portfolio.Application.Queries;

public abstract class QueryBase<TResponse> : IRequest<TResponse>;