namespace API.Contract.Users.Responses;

public record AuthenticateResponse(
  string Id,
  string AccessToken,
  string RefreshToken,
  bool IsAdmin);
