namespace Common.Mediator;

public interface IRequestHandler<in T>
  where T : class
{
  Task<Result> Handle(T request);
}
