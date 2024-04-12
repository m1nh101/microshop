using Microsoft.Extensions.Configuration;
using Microsoft.Graph;

namespace Common.Mail;

public class Mailer : IMailer
{
  private readonly GraphServiceClient _client;
  private readonly IConfiguration _configuration;

  public Mailer(GraphServiceClient client, IConfiguration configuration)
  {
    _client = client;
    _configuration = configuration;
  }

  public async Task Send(MailTemplateBuilder builder)
  {
    await _client.Users[_configuration["AZURE_MAIL_NAME"]].SendMail.PostAsync(builder.Build());
  }

  public async Task Send(params MailTemplateBuilder[] mailTemplateBuilders)
  {
    foreach(var mailTemplateBuilder in mailTemplateBuilders)
      await Send(mailTemplateBuilder);
  }
}
