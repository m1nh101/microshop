namespace User.API.Http.Responses;

public record AuthenticateResponse(
  string Id,
  string AccessToken,
  string RefreshToken);
