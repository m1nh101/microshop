using System.Text;

namespace User.API.Application.Helpers;

public static class ConfirmationCodeGenerator
{
  public static string Generate()
  {
    var code = new StringBuilder();
    var random = new Random();

    for (int i = 0; i < 8; i++) { }
      code.Append(random.Next(10));

    return code.ToString();
  }
}
