using System.Reflection;

namespace Common.EventBus;

public class RunningAssembly
{
  public Assembly Assembly { get; set; } = null!;
}