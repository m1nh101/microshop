using Common.Mail;

namespace User.API.Application.MailTemplates;

public class NotifyUserCredentialRemovedMailTemplateBuilder : MailTemplateBuilder
{
  public override MailTemplateBuilder WithBody(IDictionary<string, string> context)
  {
    var name = context["name"];

    Content = $"""
        <h3>Xin chào {name},</h3>
        <p>Tài khoản của b đã bị xóa do không thực hiện xác nhận email sau 7 ngày kể từ khi đăng ký thành công.</p>
      """;

    return this;
  }
}
