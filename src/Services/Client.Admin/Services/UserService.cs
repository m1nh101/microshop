using API.Contract.Users.Requests;
using API.Contract.Users.Responses;
using Common;

namespace Client.Admin.Services;

public class UserService : BaseService
{
  public UserService(HttpClient client) : base(client)
  {
  }

  public Task<Result<AuthenticateResponse>> Authenticate(AuthenticateRequest req)
  {
    return MakeRequest<AuthenticateResponse>(req, "/user-api/users/auth");
  }

  public Task<Result<RegisterResponse>> Register(RegisterRequest req)
  {
    return MakeRequest<RegisterResponse>(req, "/user-api/users/register");
  }
}
