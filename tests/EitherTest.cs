using Sork.Funk;

namespace Sork.Funk.Tests;

public class EitherTest
{

    [Fact]
    public void MapLeft_OnLeftInstance_ShouldApplyFunction()
    {
        var left = new Left<int, string>(10);
        Func<int, double> mapFunction = x => x / 2.0;

        var result = left.MapLeft(mapFunction);

        Assert.IsType<Left<double, string>>(result);
        var leftResult = (Left<double, string>)result;
        Assert.Equal(5.0, leftResult.Reduce(s => 0.0));
    }

    [Fact]
    public void MapRight_OnLeftInstance_ShouldNotApplyFunction()
    {
        var left = new Left<int, string>(10);
        Func<string, string> mapFunction = s => s.ToUpper();

        var result = left.MapRight(mapFunction);

        Assert.IsType<Left<int, string>>(result);
        var leftResult = (Left<int, string>)result;
        Assert.Equal(10, leftResult.Reduce(s => 0));
    }

    [Fact]
    public void Reduce_OnRightInstance_ShouldApplyFunction()
    {
        var right = new Right<int, string>("test");
        Func<string, int> mapFunction = s => s.Length;

        var result = right.Reduce(mapFunction);

        Assert.Equal(4, result);
    }

    [Fact]
    public void MapRight_OnRightInstance_ShouldApplyFunction()
    {
        var right = new Right<int, string>("test");
        Func<string, string> mapFunction = s => s.ToUpper();

        var result = right.MapRight(mapFunction);

        Assert.IsType<Right<int, string>>(result);
        Assert.Equal("TEST", result.Match(x => "failure", x => x));
    }

    [Fact]
    public void MapLeft_OnRight_ShouldReduceToRight()
    {
        var right = new Right<int, string>("test");

        var result = right.MapLeft(x => $"Failed with {x}").Reduce(s => s);
        
        Assert.Equal("test", result);
    }

    [Fact]
    public void Comparison_WithEqualData_ShouldBeEqual()
    {
        var left1 = new Left<int, string>(20);
        var left2 = new Left<int, string>(20);
        var right1 = new Right<int, string>("test");
        var right2 = new Right<int, string>("test");
        
        Assert.Equal(left1, left2);
        Assert.Equal(right1, right2);
    }

    [Fact]
    public void Comparison_WithDifferentData_ShouldNotBeEqual()
    {
        Either<int, string> left1 = new Left<int, string>(2);
        Either<int,string> left2 = new Left<int, string>(3);
        Either<int, string> right1 = new Right<int, string>("test");
        Either<int,string> right2 = new Right<int, string>("testtest");
        
        Assert.NotEqual(left1, left2);
        Assert.NotEqual(right1, right2);
        Assert.NotEqual(left1, right2);
    }
}