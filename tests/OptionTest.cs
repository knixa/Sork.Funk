using Microsoft.VisualBasic.CompilerServices;
using System.ComponentModel.Design;

namespace Sork.Funk.Tests;

public class OptionTest
{
    [Fact]
    public void Some_Value_ReturnsIsSomeTrue()
    {
         Option<int>.Some(42).IsSome.Should().BeTrue();
    }

    [Fact]
    public void None_ReturnsIsNoneTrue()
    {
        Option<int>.None.IsNone.Should().BeTrue();
    }

    [Fact]
    public void Map_SomeValue_ReturnsMappedValue()
    {
        Option<int>.Some(10)
            .Map(x => x * 2)
            .Match(some => some, () => -100)
            .Should().BePositive()
            .And.Be(20);
    }

    [Fact]
    public void Bind_SomeValue_ReturnsNewOption()
    {
        Option<int>.Some(5)
            .Bind(x => Option<string>.Some($"{x}"))
            .Match(some => some, () => "ALL WRONG")
            .Should().Be("5");
    }

    [Fact]
    public void Bind_None_ReturnsNone()
    {
        Option<int>.None.Map(x => x * 100)
            .IsNone
            .Should().BeTrue();
    }

    [Fact]
    public void Some_WithNullInput_ShouldThrow()
    {
        Assert.Throws<NullReferenceException>( ()=> Option<string>.Some(null).Map(x => x.Length));
        Assert.Throws<InvalidOperationException>( ()=> Option<int?>.Some(null).Map(x => x.Value));
        Assert.Throws<InvalidOperationException>( ()=> Option<bool?>.Some(null).Map(x => x.Value));
        Assert.Throws<NullReferenceException>( ()=> Option<TestClass>.Some(null).Map(x => x.GetType().Name));
        Assert.Throws<InvalidOperationException>( ()=> Option<TestStruct?>.Some(null).Map(x => x.Value));
    }

    [Fact]
    public void Some_WithString_ShouldEitherRight()
    {
        Option<string>.Some("test")
            .ToEither(() => new Exception())
            .Match(l => l.Message, r => r)
            .Should().Be("test");
    }

    [Fact]
    public void Some_WithNone_ShouldEitherLeft()
    {
        Option<int>.None.ToEither(() => new NullReferenceException())
            .Reduce(f => throw new AggregateException())
            .Should().BeOfType<NullReferenceException>();
    }
    
    private struct TestStruct{}
    private class TestClass{}
}