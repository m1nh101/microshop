﻿namespace API.Contract.Users.Requests;

public record RequireAccessTokenRequest
{
  public required string RefreshToken { get; init; }

  public static bool TryParse(string? input, out RequireAccessTokenRequest request)
  {
    request = new RequireAccessTokenRequest { RefreshToken = string.Empty };

    if (string.IsNullOrEmpty(input))
      return false;

    request = new RequireAccessTokenRequest { RefreshToken = input };
    return true;
  }
}