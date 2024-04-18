namespace Common.Contracts;

public interface ICreateable
{
  DateTime CreatedAt { get; set; }
  string CreatedBy { get; set; }
}
