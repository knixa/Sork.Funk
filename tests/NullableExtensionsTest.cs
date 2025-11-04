namespace Sork.Funk.Tests;

public class NullableExtensionsTest
{
    [Fact]
    public void ToOptions_WithNull_ShouldReturnNone()
    {
        int? val = null;

        Assert.True(val.ToOption().IsNone);
    }

    [Fact]
    public void ToOptions_WithValue_ShouldBeSome()
    {
        bool? val = true;

        Assert.True(val.ToOption().IsSome);
    }

    [Fact]
    public void ToOptions_WithNullClass_ShouldBeNone()
    {
        List<int> val = null;

        Assert.True(val.ToOption().IsNone);
    }

    [Fact]
    public void ToOptions_WithList_ShouldBeSome() =>Assert.True( new List<int> { 1, 2, 3 }.ToOption().IsSome);
}
