namespace Common.Mediator;

public interface IRequestHandler<in T>
  where T : class
{
  Task<object> Handle(T request);
}
