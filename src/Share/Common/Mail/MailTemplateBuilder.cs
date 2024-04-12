using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;

namespace Common.Mail;

public abstract class MailTemplateBuilder
{
  private readonly SendMailPostRequestBody _payload;
  private readonly List<Recipient> _receivers;
  private string _subject = "";
  protected string Content = "";

  public MailTemplateBuilder()
  {
    _payload = new SendMailPostRequestBody();
    _receivers = [];
  }

  public abstract MailTemplateBuilder WithBody(IDictionary<string, string> context);

  public MailTemplateBuilder WithReceiver(params string[] emailAddresses)
  {
    _receivers.AddRange(emailAddresses.Select(e => new Recipient
    {
      EmailAddress = new EmailAddress
      {
        Address = e,
      }
    }));

    return this;
  }

  public MailTemplateBuilder WithSubject(string subject)
  {
    _subject = subject;
    return this;
  }

  public SendMailPostRequestBody Build()
  {
    var message = new Message
    {
      Body = new ItemBody
      {
        Content = Content,
        ContentType = BodyType.Html
      },
      ToRecipients = _receivers,
      Subject = _subject,
      IsDraft = string.IsNullOrEmpty(_subject)
    };

    _payload.SaveToSentItems = true;
    _payload.Message = message;

    return _payload;
  }
}