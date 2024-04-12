using Common.EventBus;
using User.API.Domain.Events;

namespace User.API.Domain.Entities;

public class User : DomainEvent
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
    CreateAt = DateTime.Now;
  }

  public string Id { get; private set; } = string.Empty;
  public string Username { get; private set; } = string.Empty;
  public string Name { get; private set; } = string.Empty;
  public string Email { get; private set; } = string.Empty;
  public string Phone { get; private set; } = string.Empty;
  public string Password { get; private set; } = string.Empty;
  public bool IsConfirm { get; private set; } = false;
  public DateTime CreateAt { get; private set; }

  public virtual ICollection<UserRole> UserRoles { get; private set; } = [];
  public virtual ICollection<Role> Roles { get; private set; } = [];

  public void AddToRole(string roleId) => UserRoles.Add(new UserRole(roleId));

  public void Confirm()
  {
    IsConfirm = true;
    AddEvent(new UserConfirmedEvent(Id));
  }
}
