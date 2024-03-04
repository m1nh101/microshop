using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Auth;

public static class AuthConfiguration
{
  public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
  {
    services.Configure<TokenOption>(opt =>
    {
      opt.ExpiredIn = Convert.ToInt32(configuration["Token:ExpiredIn"]);
      opt.SecretKey = configuration["Token:SecretKey"] ?? throw new NullReferenceException();
    });

    services.AddAuthentication(opt =>
    {
      opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })
      .AddJwtBearer(opt =>
      {
        var key = Encoding.UTF8.GetBytes(configuration["Token:SecretKey"]
          ?? throw new ArgumentNullException("no secret key found"));

        opt.RequireHttpsMetadata = false;
        opt.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateLifetime = true,
          ValidateIssuer = false,
          ValidateAudience = false,
        };

        opt.Events = new JwtBearerEvents
        {
          OnMessageReceived = context =>
          {
            var token = context.Request.Cookies["access_token"]
              ?? context.Request.Headers.Authorization.ToString()
              ?? string.Empty;

            context.Token = token.Replace("Bearer ", string.Empty);

            return Task.CompletedTask;
          }
        };
      });

    services.AddAuthorizationBuilder()
      .AddPolicy(PolicyName.SuperUser, policy =>
      {
        policy.RequireRole(PolicyName.SuperUser);
      })
      .AddPolicy(PolicyName.SignedInUser, policy =>
      {
        policy.RequireAuthenticatedUser();
      });

    services.AddScoped<IUserSessionContext, UserSessionContext>();

    return services;
  }
}
