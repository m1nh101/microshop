using Newtonsoft.Json;
using System.Text;
using User.API.Infrastructure.Caching.Models;

namespace User.API.Application.Extensions;

public static class Base64Extension
{
  public static string ToBase64(this UserToken token)
  {
    var plainText = JsonConvert.SerializeObject(token);
    var bytes = Encoding.UTF8.GetBytes(plainText);
    return Convert.ToBase64String(bytes);
  }

  public static UserToken FromBase64(this string base64String)
  {
    var base64EncodedBytes = Convert.FromBase64String(base64String);
    var plainText = Encoding.UTF8.GetString(base64EncodedBytes);
    return JsonConvert.DeserializeObject<UserToken>(plainText)!;
  }
}
