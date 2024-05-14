using Sork.Funk;

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
}