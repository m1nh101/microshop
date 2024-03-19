namespace User.API.Infrastructure.Entities;

public class User
{
  private User() { }

  public User(
    string username,
    string name,
    string email,
    string phone,
    string password)
  {
    Username = username;
    Name = name;
    Email = email;
    Phone = phone;
    Password = password;
    Id = Guid.NewGuid().ToString();
  }

  public string Id { get; private set; } = string.Empty;
  public string Username { get; private set; } = string.Empty;
  public string Name { get; private set; } = string.Empty;
  public string Email { get; private set; } = string.Empty;
  public string Phone { get; private set; } = string.Empty;
  public string Password { get; private set; } = string.Empty;

  public virtual ICollection<UserRole> Roles { get; private set; } = [];
  
  public virtual ICollection<CardPayment>

  public void AddToRole(string roleId) => Roles.Add(new UserRole(roleId));
}
