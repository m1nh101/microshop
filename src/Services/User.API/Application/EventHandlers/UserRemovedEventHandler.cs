using Common.EventBus;
using Common.Mail;
using User.API.Application.MailTemplates;
using User.API.Domain.Events;

namespace User.API.Application.EventHandlers;

public sealed class UserRemovedEventHandler : IDomainEventHandler<UserRemovedEvent>
{
  private readonly IMailer _mailer;

  public UserRemovedEventHandler(IMailer mailer)
  {
    _mailer = mailer;
  }

  public async Task Handle(UserRemovedEvent @event)
  {
    var context = new Dictionary<string, string>()
    {
      { "name", @event.User.Name },
    };

    var mailTemplate = new NotifyUserCredentialRemovedMailTemplateBuilder()
      .WithSubject("Tài khoản của bạn đã bị vô hiệu hóa")
      .WithReceiver(@event.User.Email)
      .WithBody(context);

    await _mailer.Send(mailTemplate);
  }
}
