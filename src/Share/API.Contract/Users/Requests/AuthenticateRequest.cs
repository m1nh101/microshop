namespace API.Contract.Users.Requests;

public record AuthenticateRequest(
  string Username,
  string Password);
