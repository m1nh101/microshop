namespace Common;

public static class Helper
{
  public static string GetAssemblyName<TType>()
  {
    return typeof(TType).Assembly.GetName().Name ?? throw new NullReferenceException();
  }

  public static string GetAssemblyName(Type type)
  {
    return type.Assembly.GetName().Name ?? throw new NullReferenceException();
  }
}
