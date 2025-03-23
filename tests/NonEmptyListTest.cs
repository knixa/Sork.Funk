using Sork.Funk;

namespace Sork.Funk.Tests;

public class NonEmptyListTest
{
    [Fact]
    public void Create_WithNonEmptyEnumerable_ShouldSucceed()
    {
        var items = new List<int> { 1, 2, 3 };

        var nonEmptyList = NonEmptyList<int>.Create(items);

        Assert.NotNull(nonEmptyList);
        Assert.Equal(3, nonEmptyList.Count);
        Assert.Equal(1, nonEmptyList.Head);
        Assert.Equal(new List<int> { 2, 3 }, nonEmptyList.Tail);
    }

    [Fact]
    public void Create_WithEmptyEnumerable_ShouldThrowArgumentException()
    {
        var items = new List<int>();

        Assert.Throws<ArgumentException>(() => NonEmptyList<int>.Create(items));
    }

    [Fact]
    public void Create_WithNullEnumerable_ShouldThrowArgumentNullException()
    {
        IEnumerable<int> items = null;

        Assert.Throws<ArgumentNullException>(() => NonEmptyList<int>.Create(items));
    }

    [Fact]
    public void Create_WithHeadAndTail_ShouldSucceed()
    {
        var nonEmptyList = NonEmptyList<int>.Create(1, 2, 3, 4);

        Assert.NotNull(nonEmptyList);
        Assert.Equal(4, nonEmptyList.Count);
        Assert.Equal(1, nonEmptyList.Head);
        Assert.Equal(new List<int> { 2, 3, 4 }, nonEmptyList.Tail);
    }

    [Fact]
    public void Head_ShouldReturnFirstElement()
    {
        var items = new List<string> { "a", "b", "c" };
        var nonEmptyList = NonEmptyList<string>.Create(items);

        var head = nonEmptyList.Head;

        Assert.Equal("a", head);
    }

    [Fact]
    public void Tail_ShouldReturnAllButFirstElement()
    {
        var items = new List<int> { 10, 20, 30 };
        var nonEmptyList = NonEmptyList<int>.Create(items);

        var tail = nonEmptyList.Tail;

        Assert.Equal(new List<int> { 20, 30 }, tail);
    }

    [Fact]
    public void Indexer_ShouldReturnElementAtIndex()
    {
        var items = new List<int> { 100, 200, 300 };
        var nonEmptyList = NonEmptyList<int>.Create(items);

        var secondElement = nonEmptyList[1];

        Assert.Equal(200, secondElement);
    }

    [Fact]
    public void Count_ShouldReturnCorrectNumberOfElements()
    {
        var items = new List<int> { 1, 2, 3, 4 };
        var nonEmptyList = NonEmptyList<int>.Create(items);

        var count = nonEmptyList.Count;

        Assert.Equal(4, count);
    }

    [Fact]
    public void Enumerator_ShouldIterateThroughAllElements()
    {
        var items = new List<string> { "x", "y", "z" };
        var nonEmptyList = NonEmptyList<string>.Create(items);

        var result = nonEmptyList.ToList();

        Assert.Equal(items, result);
    }
}
