using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;

namespace Common.Mail;

public static class AzureMailConfiguration
{
  public static IServiceCollection AddAzureMail(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped(sp =>
    {
      string[] defaultScope = ["https://graph.microsoft.com/.default"];
      var tenantId = configuration["AZURE_TENANT_ID"] ?? throw new NullReferenceException();
      var clientId = configuration["AZURE_CLIENT_ID"] ?? throw new NullReferenceException();
      var clientSecret = configuration["AZURE_CLIENT_SECRET"] ?? throw new NullReferenceException();
      var options = new ClientCertificateCredentialOptions
      {
        AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
      };

      var clientCredential = new ClientSecretCredential(
        tenantId, clientId, clientSecret, options);

      return new GraphServiceClient(clientCredential, defaultScope);
    });

    services.AddScoped<IMailer, Mailer>();

    return services;
  }
}
