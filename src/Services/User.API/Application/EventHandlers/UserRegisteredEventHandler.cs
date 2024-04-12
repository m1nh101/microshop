using Common;
using Common.EventBus;
using Common.Mail;
using User.API.Application.CachingModels;
using User.API.Application.Contracts;
using User.API.Application.Helpers;
using User.API.Application.MailTemplates;
using User.API.Domain.Events;

namespace User.API.Application.EventHandlers;

public class UserRegisteredEventHandler : IDomainEventHandler<UserRegisteredEvent>
{
  private readonly IMailer _mailer;
  private readonly IUserConfirmationStorage _storage;

  public UserRegisteredEventHandler(IMailer mailer,
    IUserConfirmationStorage storage)
  {
    _mailer = mailer;
    _storage = storage;
  }

  public async Task Handle(UserRegisteredEvent @event)
  {
    var confirmationCode = ConfirmationCodeGenerator.Generate();
    await _storage.Add(new UserConfirmation
    {
      Id = @event.User.Id,
      ConfirmationCode = confirmationCode,
      Token = Helper.GenerateHash(new { @event.User.Id, ConfirmationCode = confirmationCode }),
    });

    var bodyContext = new Dictionary<string, string>
    {
      { "confirmCode", confirmationCode },
      { "name", @event.User.Name},
    };
    var mailTemplate = new ConfirmUserMailTemplateBuilder()
      .WithSubject("Xác nhận người dùng microshop")
      .WithReceiver(@event.User.Email)
      .WithBody(bodyContext);

    await _mailer.Send(mailTemplate);
  }
}
