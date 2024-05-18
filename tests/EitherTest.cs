using Sork.Funk;

namespace Sork.Funk.Tests;

public class EitherTest
{

    [Fact]
    public void Map_OnRightInstance_ShouldApplyFunction()
    {
        var right = new Right<string, int>(10);
        double Halve(int x) => x / 2.0;

        var result = right.Map(Halve);

        Assert.IsType<Right<string, double>>(result);
        Assert.Equal(5.0, result.Match(l => -0, r => r));
    }

    [Fact]
    public void Map_OnLeftInstance_ShouldNotApplyFunction()
    {
        var left = new Left<int, string>(10);

        var result = left.Map(s => s.ToUpper());

        Assert.IsType<Left<int, string>>(result);
        var leftResult = (Left<int, string>)result;
        Assert.Equal(10, leftResult.Reduce(s => 0));
    }

    [Fact]
    public void Reduce_OnRightInstance_ShouldApplyFunction()
    {
        var right = new Right<int, string>("test");

        var result = right.Reduce(s => s.Length);

        Assert.Equal(4, result);
    }

    [Fact]
    public void MapToUpper_OnRightInstance_ShouldApplyFunction()
    {
        var right = new Right<int, string>("test");

        var result = right.Map(s => s.ToUpper());

        Assert.IsType<Right<int, string>>(result);
        Assert.Equal("TEST", result.Match(x => "failure", x => x));
    }

    [Fact]
    public void Reduce_OnLeft_ShouldReturnLeft()
    {
        var first = new Left<string, string>("test");
        var second = new Left<int, int[]>(-1);

        Assert.Equal("test", first.Reduce(x => x.ToUpper()));
        Assert.Equal(-1, second.Reduce(r => r.Sum()));
    }

    [Fact]
    public void ReduceOnRight_ShouldReturnReduce()
    {
        var first = new Right<string, string>("test");
        var second = new Right<int, int[]>([1, 2, 3, 4, 5]);
        
        Assert.Equal("TEST", first.Reduce(x => x.ToUpper()));
        Assert.Equal(15, second.Reduce(r => r.Sum()));

    }

    [Fact]
    public void Comparison_WithEqualData_ShouldBeEqual()
    {
        var left1 = GetLeft<int, string>(20);
        var left2 = GetLeft<int, string>(20);
        var right1 = GetRight<int, string>("test");
        var right2 = GetRight<int, string>("test");
        
        Assert.Equal(left1, left2);
        Assert.Equal(right1, right2);
    }

    internal static Either<L, R> GetLeft<L, R>(L val) => val;
    internal static Either<L, R> GetRight<L, R>(R val) => val; 

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