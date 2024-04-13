namespace User.API.Application.Contracts;

public interface IRemoveable
{
  bool IsDeleted { get; }
  void Remove();
}
