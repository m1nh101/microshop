﻿using Common;

namespace User.API;

internal static class Errors
{
  public static readonly Error DuplicateEmail = new("User.Email", "Email has been used");
  public static readonly Error DuplicatePhone = new("User.Phone", "Phone number has been used");
  public static readonly Error DuplicateUsername = new("User.Username", "User name has been used");
  public static readonly Error ConfirmPasswordNotMatch = new("User.Password", "Confirm password not match with password");
  public static readonly Error WrongPassword = new("User.Password", "Password is incorrect");
  public static readonly Error RefreshTokenIsNotValid = new("User.Token", "Refresh token is invalid");
}
