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
    public void Create_WithEmptyEnumerable_ShouldThrowArgumentException() =>
        Assert.Throws<ArgumentException>(() => NonEmptyList<int>.Create([]));

    [Fact]
    public void Create_WithNullEnumerable_ShouldThrowArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => NonEmptyList<int>.Create(null!));

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

    [Fact]
    public void Compare_WithSameValues_ShouldBeEqual()
    {
        var int01 = NonEmptyList<int>.Create(1, 3, 7);
        var int02 = NonEmptyList<int>.Create(1, 3, 7);

        var str01 = NonEmptyList<string>.Create("1", "test", "compare");
        var str02 = NonEmptyList<string>.Create("1", "test", "compare");
        
        Assert.True(int01 == int02);
        Assert.True(int01.Equals(int01));
        Assert.True(int01.Equals((object)int02));
        Assert.Equal(int01.GetHashCode(), int02.GetHashCode());
        
        Assert.True(str01 == str02);
        Assert.True(str01.Equals(str01));
        Assert.True(str01.Equals((object)str02));
        Assert.Equal(str01.GetHashCode(), str02.GetHashCode());
    }
    [Fact]
    public void Compare_WithDifferentValues_ShouldNotBeEqual()
    {
        var int01 = NonEmptyList<int>.Create(1,  7);
        var int02 = NonEmptyList<int>.Create(1, 3, 7);

        var str01 = NonEmptyList<string>.Create("1", "test", "compare");
        var str02 = NonEmptyList<string>.Create("2", "test", "compare");
        
        Assert.True(int01 != int02);
        Assert.True(int01 != null);
        Assert.False(int01.Equals((object)int02));
        Assert.NotEqual(int01.GetHashCode(), int02.GetHashCode());
        
        Assert.True(str01 != str02);
        Assert.True(str01 != null);
        Assert.False(str01.Equals((object)str02));
        Assert.NotEqual(str01.GetHashCode(), str02.GetHashCode());
    }
}
