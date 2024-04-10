namespace Common.UnitTest;

public class Sample
{
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
}

public class ResultUnitTest
{
  [SetUp]
  public void Setup()
  {
  }

  [Test]
  public void ResultOnSuccessResultTest()
  {
    var sample = new Sample();
    var result = Result.Ok(sample);

    Assert.Multiple(() =>
    {
      Assert.That(result.Data, Is.Not.Null);
      Assert.That(result.IsSuccess, Is.True);
    });
  }

  [Test]
  public void CastingTypeOnFailureResultTest()
  {
    var result = Result.Failed(new Error("Test", "Test"));
    Assert.Multiple(() =>
    {
      Assert.That(result.Data, Is.Null);
      Assert.That(result.IsSuccess, Is.False);
      Assert.Throws<CastingDataTypeException>(() => result.As<Sample>());
    });
  }
}