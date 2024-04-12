using Common.Mail;

namespace User.API.Application.MailTemplates;

public class ForgotPasswordMailTemplateBuilder : MailTemplateBuilder
{
  public override MailTemplateBuilder WithBody(IDictionary<string, string> context)
  {
    var resetPasswordCode = context["code"];

    Content = $"""
      <div>
        <a href="" style="display:block; padding: 10px; border-radius: 8px; background-color: red; text-decoration: none; color: white; font-size: 16; font-weight: 600; font-family: san-serif; ">Nhấn vào đây để lấy lại mật khẩu</a>

        <p>Hoặc sử dụng mã sau để khôi phục mật khẩu: <span style="font-weight: bold; font-size: 16px">{resetPasswordCode}</span></p>

        <p>Mã khôi phục có hiệu lực trong vòng 10 phút</p>
      </div>
      """;

    return this;
  }
}
