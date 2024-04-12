namespace Common.Mail;

public interface IMailer
{
  Task Send(MailTemplateBuilder builder);
  Task Send(params MailTemplateBuilder[] mailTemplateBuilders);
}
