namespace Common;

public class CastingDataTypeException : Exception
{
  public CastingDataTypeException(Type dataType)
    :base($"cannot cast object to {dataType.Name}")
  {
  }
}