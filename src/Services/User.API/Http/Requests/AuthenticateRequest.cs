namespace User.API.Http.Requests;

public record AuthenticateRequest(
  string Username,
  string Password);
