using Sork.Funk;

namespace Sork.Funk.Tests;

public class OptionExtensionsTest
{
    [Fact]
    public void Unwrap_WithOnlyNone_ShouldBeEmpty()
    {
        new List<Option<int>> { Option<int>.None, Option<int>.None }.Unwrap().Should().BeEmpty();
    }

    [Fact]
    public void Unwrap_WithOnlySome_ShouldNotBeEmpty()
    {
        new List<Option<int>> { Option<int>.Some(3), Option<int>.Some(2) }
            .Unwrap()
            .Should()
            .NotBeEmpty().And
            .HaveCount(2);
    }

    [Fact]
    public void unwrap_WithMixed_ShouldHaveCorrectCount()
    {
        new List<Option<string>>
            {
                Option<string>.Some("test"),
                Option<string>.None,
                Option<string>.Some("next"),
                Option<string>.Some("again"),
                Option<string>.None
            }.Unwrap()
            .Should().NotBeEmpty().And.HaveCount(3);
    }
}