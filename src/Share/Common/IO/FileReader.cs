namespace Common.IO;

public abstract class FileReader
{
  public abstract IEnumerable<T> Read<T>(string path);
}
