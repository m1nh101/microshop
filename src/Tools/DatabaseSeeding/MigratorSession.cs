using Common.Auth;

namespace DatabaseSeeding;
public class MigratorSession : IUserSessionContext
{
  public string UserId => "Migrator";

  public string Name => "Migrator";
}