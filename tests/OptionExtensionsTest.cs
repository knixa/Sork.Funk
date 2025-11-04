namespace Sork.Funk.Tests;

public class OptionExtensionsTest
{
    [Fact]
    public void Unwrap_WithOnlyNone_ShouldBeEmpty() =>
        Assert.Empty(new List<Option<int>> { Option<int>.None, Option<int>.None }.Unwrap());

    [Fact]
    public void Unwrap_WithOnlySome_ShouldNotBeEmpty() =>
        Assert.Equal(2, new List<Option<int>> { Option<int>.Some(3), Option<int>.Some(2) }
            .Unwrap().Count());

    [Fact]
    public void Unwrap_WithMixed_ShouldHaveCorrectCount() =>
        Assert.Equal(3,
            new List<Option<string>>
            {
                Option<string>.Some("test"),
                Option<string>.None,
                Option<string>.Some("next"),
                Option<string>.Some("again"),
                Option<string>.None
            }.Unwrap().Count());

    [Fact]
    public void ToOption_WithNullCollection_ShouldReturnNone()
    {
        IEnumerable<int>? collection = null;

        var result = collection.ToNonEmptyOption();

        Assert.True(result.IsNone);
        Assert.False(result.IsSome);
    }

    [Fact]
    public void ToOption_WithEmptyCollection_ShouldReturnNone()
    {
        var collection = new List<int>();

        var result = collection.ToNonEmptyOption();

        Assert.True(result.IsNone);
        Assert.False(result.IsSome);
    }

    [Fact]
    public void ToOption_WithNonEmptyCollection_ShouldReturnSome()
    {
        var collection = new List<int> { 1, 2, 3 };

        var result = collection.ToNonEmptyOption();

        Assert.True(result.IsSome);
        Assert.False(result.IsNone);

        var nonEmptyList = result.Match(
            some: x => x,
            none: () => throw new InvalidOperationException("Unexpected None in test.")
        );

        Assert.Equal(3, nonEmptyList.Count);
        Assert.Equal(1, nonEmptyList.Head);
        Assert.Equal(new List<int> { 2, 3 }, nonEmptyList.Tail);
    }

    [Fact]
    public void ToOption_WithSingleElementCollection_ShouldReturnSomeWithOneElement()
    {
        var collection = new List<string> { "onlyItem" };

        var result = collection.ToNonEmptyOption();

        Assert.True(result.IsSome);

        var nonEmptyList = result.Match(
            some: x => x,
            none: () => throw new InvalidOperationException("Unexpected None in test.")
        );

        Assert.Single(nonEmptyList);
        Assert.Equal("onlyItem", nonEmptyList.Head);
        Assert.Empty(nonEmptyList.Tail);
    }

    [Fact]
    public void ToOption_WithLargeCollection_ShouldHandleCorrectly()
    {
        var collection = new List<int>();
        for (var i = 1; i <= 1000; i++)
        {
            collection.Add(i);
        }

        var result = collection.ToNonEmptyOption();

        Assert.True(result.IsSome);

        var nonEmptyList = result.Match(
            some: x => x,
            none: () => throw new InvalidOperationException("Unexpected None in test.")
        );

        Assert.Equal(1000, nonEmptyList.Count);
        Assert.Equal(1, nonEmptyList.Head);
        Assert.Equal(999, nonEmptyList.Tail.Count);
    }
}
