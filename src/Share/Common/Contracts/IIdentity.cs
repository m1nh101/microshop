namespace Common.Contracts;

public interface IIdentity<TKey>
{
  TKey Id { get; }
}
