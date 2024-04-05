using API.Contract.Users.Requests;
using API.Contract.Users.Responses;
using Common;
using Common.Mediator;
using Microsoft.EntityFrameworkCore;
using User.API.Application.Contracts;
using User.API.Infrastructure.Database;

namespace User.API.Application.Handlers;

public class RegisterHandler : IRequestHandler<RegisterRequest>
{
  private readonly IPasswordGenerator _passwordGenerator;
  private readonly UserDbContext _context;

  public RegisterHandler(
    IPasswordGenerator passwordGenerator,
    UserDbContext context)
  {
    _passwordGenerator = passwordGenerator;
    _context = context;
  }

  public async Task<object> Handle(RegisterRequest request)
  {
    var errors = new List<Error>();

    // check user info
    var usernameValidation = await _context.Users.AsNoTracking()
      .AnyAsync(e => e.Username == request.Username);
    var phoneValidation = await _context.Users.AsNoTracking()
      .AnyAsync(e => e.Phone == request.Phone);
    var emailValidation = await _context.Users.AsNoTracking()
      .AnyAsync(e => e.Email == request.Email);

    if (usernameValidation)
      errors.Add(Errors.DuplicateUsername);
    if (phoneValidation)
      errors.Add(Errors.DuplicatePhone);
    if (emailValidation)
      errors.Add(Errors.DuplicateEmail);
    if (request.Password != request.ConfirmPassword)
      errors.Add(Errors.ConfirmPasswordNotMatch);

    if (errors.Count != 0)
      return errors;

    var user = new Infrastructure.Entities.User(
      username: request.Username,
      name: request.Name,
      email: request.Email,
      phone: request.Phone,
      password: _passwordGenerator.Generate(request.Password));

    var role = await _context.Roles.AsNoTracking()
      .FirstOrDefaultAsync(e => e.Name.Equals("user"))
      ?? throw new NullReferenceException();

    user.AddToRole(role.Id);

    await _context.Users.AddAsync(user);
    await _context.SaveChangesAsync();

    return new RegisterResponse();
  }
}
