namespace Common.Contracts;

public interface IAuditable : ICreateable
{
  DateTime? ModifiedAt { get; set; }
  string? ModifiedBy { get; set; }
}