using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

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

  public static string GenerateHash(object obj)
  {
    var plainText = JsonConvert.SerializeObject(obj);
    var bytes = Encoding.UTF8.GetBytes(plainText);
    var hash = MD5.HashData(bytes);
    var result = new StringBuilder();

    foreach (var c in hash)
      result.Append(c.ToString("x2"));

    return result.ToString();
  }
}
