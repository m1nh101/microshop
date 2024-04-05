namespace API.Contract.Users.Requests;

public record RequireAccessTokenRequest(
  string RefreshToken);