using Common;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using User.API.Application.Contracts;
using User.API.Http.Requests;
using User.API.Http.Responses;
using User.API.Infrastructure.Database;

namespace User.API.Http.Endpoints;

[AllowAnonymous]
[HttpPost("/api/users/register")]
public sealed class RegisterEndpoint : Endpoint<RegisterRequest, Result<RegisterResponse>>
{
  private readonly IPasswordGenerator _passwordGenerator;
  private readonly UserDbContext _context;

  public RegisterEndpoint(UserDbContext context,
    IPasswordGenerator passwordGenerator)
  {
    _context = context;
    _passwordGenerator = passwordGenerator;
  }

  public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
  {
    var errors = new List<Error>();

    // check user info
    var usernameValidation = await _context.Users.AsNoTracking()
      .AnyAsync(e => e.Username == req.Username, ct);
    var phoneValidation = await _context.Users.AsNoTracking()
      .AnyAsync(e => e.Phone == req.Phone, ct);
    var emailValidation = await _context.Users.AsNoTracking()
      .AnyAsync(e => e.Email == req.Email, ct);

    if (usernameValidation)
      errors.Add(Errors.DuplicateUsername);
    if (phoneValidation)
      errors.Add(Errors.DuplicatePhone);
    if (emailValidation)
      errors.Add(Errors.DuplicateEmail);
    if (req.Password != req.ConfirmPassword)
      errors.Add(Errors.ConfirmPasswordNotMatch);

    if(errors.Count != 0)
    {
      await SendAsync(
        response: errors,
        statusCode: 200,
        cancellation: ct);
      return;
    }

    var user = new Infrastructure.Entities.User(
      username: req.Username,
      name: req.Name,
      email: req.Email,
      phone: req.Phone,
      password: _passwordGenerator.Generate(req.Password));

    var role = await _context.Roles.AsNoTracking()
      .FirstOrDefaultAsync(e => e.Name.Equals("user"), ct)
      ?? throw new NullReferenceException();

    user.AddToRole(role.Id);

    await _context.Users.AddAsync(user, ct);
    await _context.SaveChangesAsync(ct);

    await SendAsync(
      response: new RegisterResponse(),
      statusCode: 200,
      cancellation: ct);
  }
}