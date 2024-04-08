using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Common.Auth;

public static class AuthConfiguration
{
  static readonly string _corsPolicyName = "__cors";

  public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddCors(opt =>
    {
      opt.AddPolicy(_corsPolicyName, policy =>
      {
        policy.AllowAnyHeader()
          .AllowCredentials()
          .AllowAnyMethod()
          .SetIsOriginAllowed(e => new Uri(e).Host == "localhost");
      });
    });

    services.AddHttpContextAccessor();

    services.AddCookiePolicy(opt =>
    {
      opt.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
      opt.Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
      opt.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
    });

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

  public static IApplicationBuilder UseAuth(this WebApplication app)
  {
    app.UseCors(_corsPolicyName);
    app.UseCookiePolicy();
    app.UseAuthentication();
    app.UseAuthorization();

    return app;
  }
}
