namespace Common.Mediator;

public interface IMediator
{
  IMediator Send<TRequest>(TRequest request) where TRequest : class;
  Task<TResponse> As<TResponse>();
}
