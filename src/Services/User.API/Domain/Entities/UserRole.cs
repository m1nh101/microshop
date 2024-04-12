namespace User.API.Domain.Entities;

public class UserRole
{
  private UserRole() { }
  public UserRole(string roleId)
  {
    RoleId = roleId;
  }

  public string UserId { get; private set; } = string.Empty;
  public string RoleId { get; private set; } = string.Empty;
  public User User { get; private set; } = null!;
  public Role Role { get; private set; } = null!;
}
