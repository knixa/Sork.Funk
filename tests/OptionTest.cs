using FsCheck.Xunit;

namespace Sork.Funk.Tests;

public class OptionTest
{
    [Fact]
    public void Some_Value_ReturnsIsSomeTrue() => Option<int>.Some(42).IsSome.Should().BeTrue();

    [Fact]
    public void None_ReturnsIsNoneTrue() => Option<int>.None.IsNone.Should().BeTrue();

    [Fact]
    public void Map_SomeValue_ReturnsMappedValue() =>
        Option<int>.Some(10)
            .Map(x => x * 2)
            .Match(some => some, () => -100)
            .Should().BePositive()
            .And.Be(20);

    [Property]
    public void Bind_SomeValue_ReturnsNewOption(int val) =>
        Option<int>.Some(val)
            .Bind(x => Option<string>.Some($"{x}"))
            .Match(some => some, () => "ALL WRONG")
            .Should().Be(val.ToString());

    [Fact]
    public void Bind_None_ReturnsNone() =>
        Option<int>.None.Map(x => x * 100)
            .IsNone
            .Should().BeTrue();

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
    public void Some_WithString_ShouldEitherRight(string val) =>
        Option<string>.Some(val)
            .ToEither(() => new Exception())
            .Match(l => l.Message, r => r)
            .Should().Be(val);

    [Fact]
    public void Some_WithNone_ShouldEitherLeft() =>
        Option<int>.None.ToEither(() => new NullReferenceException())
            .Reduce(f => throw new AggregateException())
            .Should().BeOfType<NullReferenceException>();

    [Property]
    public void IfNone_WithNone_ShouldReturnInput(int input) =>
        Option<int>.None.IfNone(input)
            .Should().Be(input);

    [Property]
    public void IfNone_WithSome_ShouldReturnSomeValue(int some, int x) =>
        Option<int>.Some(some)
            .IfNone(x)
            .Should().Be(some);

    private struct TestStruct
    {
    }

    private class TestClass
    {
    }
}
