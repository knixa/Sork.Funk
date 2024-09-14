using Sork.Funk;

namespace Sork.Funk.Tests;

public class NullableExtensionsTest
{
    [Fact]
    public void ToOptions_WithNull_ShouldReturnNone()
    {
        int? val = null;

        val.ToOption().IsNone.Should().BeTrue();
    }

    [Fact]
    public void ToOptions_WithValue_ShouldBeSome()
    {
        bool? val = true;

        val.ToOption().IsSome.Should().BeTrue();
    }

    [Fact]
    public void ToOptions_WithNullClass_ShouldBeNone()
    {
        List<int> val = null;

        val.ToOption().IsNone.Should().BeTrue();
    }

    [Fact]
    public void ToOptions_WithList_ShouldBeSome() => new List<int> { 1, 2, 3 }.ToOption().IsSome.Should().BeTrue();
}
