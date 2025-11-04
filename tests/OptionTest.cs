using FsCheck.Xunit;

namespace Sork.Funk.Tests;

public class OptionTest
{
    [Fact]
    public void Some_Value_ReturnsIsSomeTrue() => Assert.True(Option<int>.Some(42).IsSome);

    [Fact]
    public void None_ReturnsIsNoneTrue() => Assert.True(Option<int>.None.IsNone);

    [Fact]
    public void Map_SomeValue_ReturnsMappedValue()
    {
        var result = Option<int>.Some(10)
            .Map(x => x * 2)
            .Match(some => some, () => -100);
        Assert.Equal(20, result);
    }

    [Property]
    public void Bind_SomeValue_ReturnsNewOption(int val)
    {
        var result = Option<int>.Some(val)
            .Bind(x => Option<string>.Some($"{x}"))
            .Match(some => some, () => "ALL WRONG");
        Assert.Equal(val.ToString(), result);
    }

    [Fact]
    public void Bind_None_ReturnsNone()
    {
        var result = Option<int>.None.Map(x => x * 100).IsNone;
        Assert.True(result);
    }

    [Fact]
    public void Some_WithNullInput_ShouldThrow()
    {
#nullable disable
        Assert.Throws<NullReferenceException>(() => Option<string>.Some(null!).Map(x => x.Length));
        Assert.Throws<InvalidOperationException>(() => Option<int?>.Some(null!).Map(x => x!.Value));
        Assert.Throws<InvalidOperationException>(() => Option<bool?>.Some(null!).Map(x => x!.Value));
        Assert.Throws<NullReferenceException>(() => Option<TestClass>.Some(null!).Map(x => x.GetType().Name));
        Assert.Throws<InvalidOperationException>(() => Option<TestStruct?>.Some(null!).Map(x => x!.Value));
#nullable enable
    }

    [Property]
    public void Some_WithString_ShouldEitherRight(string val)
    {
        var result = Option<string>.Some(val)
            .ToEither(() => new Exception())
            .Match(l => l.Message, r => r);

        Assert.Equal(val, result);
    }

    [Fact]
    public void Some_WithNone_ShouldEitherLeft()
    {
        var result = Option<int>.None.ToEither(() => new NullReferenceException())
            .Reduce(f => throw new AggregateException());
        Assert.IsType<NullReferenceException>(result);
    }

    [Property]
    public void IfNone_WithNone_ShouldReturnInput(int input)
    {
        var result = Option<int>.None.IfNone(input);
        Assert.Equal(input, result);
    }

    [Property]
    public void IfNone_WithSome_ShouldReturnSomeValue(int some, int x)
    {
        var result = Option<int>.Some(some)
            .IfNone(x);
        Assert.Equal(some, result);
    }

    private struct TestStruct
    {
    }

    private class TestClass
    {
    }
}
