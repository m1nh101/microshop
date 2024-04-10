using Common.Mediator;

namespace Common.UnitTest.Moq;

public sealed record SampleCommand(
  string Title);

public sealed class SampleCommandHandler : IRequestHandler<SampleCommand>
{
  public Task<Result> Handle(SampleCommand request)
  {
    return Task.FromResult(Result.Ok(request));
  }
}
