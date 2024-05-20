using Sork.Funk;
using System.ComponentModel.Design;

namespace Sork.Funk.Tests;

public class OptionTest
{
    [Fact]
    public void Some_Value_ReturnsIsSomeTrue()
    {
        var value = 42;
        var option = Option<int>.Some(value);
        Assert.True(option.IsSome);
    }

    [Fact]
    public void None_ReturnsIsNoneTrue()
    {
        var option = Option<int>.None;
        Assert.True(option.IsNone);
    }

    [Fact]
    public void Map_SomeValue_ReturnsMappedValue()
    {
        var option = Option<int>.Some(10);
        var mappedOption = option.Map(x => x * 2);
        Assert.Equal(20, mappedOption.Match(x => x, () => 0));
    }

    [Fact]
    public void Bind_SomeValue_ReturnsNewOption()
    {
        var option = Option<int>.Some(5);
        var newOption = option.Bind(x => Option<string>.Some($"Value: {x}"));
        Assert.True(newOption.IsSome);
        Assert.Equal("Value: 5", newOption.Match(s => s, () => ""));
    }

    [Fact]
    public void Bind_None_ReturnsNone()
    {
        var option = Option<int>.None;
        var newOption = option.Bind(x => Option<string>.Some($"Value: {x}"));
        Assert.True(newOption.IsNone);
    }

    [Fact]
    public void Some_WithNullInput_ShouldThrow()
    {
        Assert.Throws<ArgumentNullException>( ()=> Option<string>.Some(null));
        Assert.Throws<ArgumentNullException>( ()=> Option<int?>.Some(null));
        Assert.Throws<ArgumentNullException>( ()=> Option<bool?>.Some(null));
        Assert.Throws<ArgumentNullException>( ()=> Option<TestClass>.Some(null));
        Assert.Throws<ArgumentNullException>( ()=> Option<TestStruct?>.Some(null));
    }

    [Fact]
    public void Some_WithString_ShouldEitherRight()
    {
        Assert.Equal("test", Option<string>.Some("test")
            .ToEither(() => new Exception())
            .Match(l => l.Message, r => r));
    }

    [Fact]
    public void Some_WithNone_ShouldEitherLeft()
    {
        Assert.IsType<NullReferenceException>(Option<int>.None
            .ToEither(() => new NullReferenceException())
            .Reduce(f => throw new CheckoutException()));
    }
    
    private struct TestStruct{}
    private class TestClass{}
}