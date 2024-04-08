using API.Contract.Users.Requests;
using API.Contract.Users.Responses;
using Common;

namespace Client.Admin.Services;

public class UserService : BaseService
{
  public UserService(HttpClient client) : base(client)
  {
  }

  public Task<Result> Authenticate(AuthenticateRequest req)
  {
    return MakeRequest<AuthenticateResponse>("/user-api/users/auth", req, HttpMethod.POST);
  }

  public Task<Result> Register(RegisterRequest req)
  {
    return MakeRequest<RegisterResponse>("/user-api/users/register", req, HttpMethod.POST);
  }

  public Task<Result> Signout()
  {
    throw new Exception();
    //return MakeRequest<RegisterResponse>("/user-api/users/sign-out", HttpMethod.DELETE);
  }
}
