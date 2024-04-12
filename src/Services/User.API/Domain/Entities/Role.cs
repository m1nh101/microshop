namespace User.API.Domain.Entities;

public class Role
{
  private Role() { }
  public Role(string name)
  {
    Id = Guid.NewGuid().ToString();
    Name = name;
  }

  public string Id { get; private set; } = string.Empty;
  public string Name { get; private set; } = string.Empty;

  public virtual ICollection<UserRole> UserRoles { get; private set; } = [];
  public virtual ICollection<User> Users { get; private set; } = [];
}
