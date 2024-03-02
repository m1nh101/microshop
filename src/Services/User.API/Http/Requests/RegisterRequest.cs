namespace User.API.Http.Requests;

public record RegisterRequest(
  string Username,
  string Password,
  string Name,
  string Email,
  string Phone,
  string ConfirmPassword) : AuthenticateRequest(Username, Password);