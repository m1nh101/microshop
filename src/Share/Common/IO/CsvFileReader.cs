using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Common.IO;

public class CsvFileReader : FileReader
{
  public override IEnumerable<T> Read<T>(string path)
  {
    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
    };

    using var reader = new StreamReader(path);
    using var csv = new CsvReader(reader, config);
    var records = csv.GetRecords<T>();

    return records.ToList();
  }
}