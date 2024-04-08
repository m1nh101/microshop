namespace Common.Mediator;

public interface IMediator
{
  Task<Result> Send<TRequest>(TRequest request) where TRequest : class;
}
