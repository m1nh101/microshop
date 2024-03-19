namespace User.API.Infrastructure.Entities;

public record CardPayment
{
  public required string CardHolder { get; init; }
  public required string CardNumber { get; init; }
  public required string SecurityNumber { get; init; }
  public required string ExpiredAt { get; init; }
}

public enum CardType
{
  Credit = 0,
  Debit = 1
}

public enum CardUnitProvider
{
  Mastercard = 0,
  Visa = 2,
  Jcb = 3
}