namespace User.API.Infrastructure.Entities;

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

  public virtual ICollection<UserRole> Users { get; private set; } = [];
}
