using Common.Mail;

namespace User.API.Application.MailTemplates;

public class ConfirmUserMailTemplateBuilder : MailTemplateBuilder
{
  public override MailTemplateBuilder WithBody(IDictionary<string, string> context)
  {
    var confirmCode = context["confirmCode"];
    var name = context["name"];

    Content = $"""
      <h3>Xin chào <span style="font-size: 16px; font-weight: 600">{name}</span></h3>
      <p>Cảm ơn bạn đã đăng ký trở thành thành viên của hệ thống, đây là mã xác thực của bạn <span style="font-size: 16px; font-weight: 500">{confirmCode}</span></p>
      <p>Mã xác thực có hiệu lực trong vòng 24h</p>
      """;

    return this;
  }
}
