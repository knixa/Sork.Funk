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
}
