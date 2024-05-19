using Sork.Funk;

namespace Sork.Funk.Tests;

public class EitherTest
{
    [Fact]
    public void Map_OnRightInstance_ShouldApplyFunction()
    {
        var right = new Either.Right<string, int>(10);
        double Halve(int x) => x / 2.0;

        var result = right.Map(Halve);

        Assert.IsType<Either.Right<string, double>>(result);
        Assert.Equal(5.0, result.Match(l => -0, r => r));
    }

    [Fact]
    public void Map_OnLeftInstance_ShouldNotApplyFunction()
    {
        var left = new Either.Left<int, string>(10);

        var result = left.Map(s => s.ToUpper());

        Assert.IsType<Either.Left<int, string>>(result);
        var leftResult = (Either.Left<int, string>)result;
        Assert.Equal(10, leftResult.Reduce(s => 0));
    }

    [Fact]
    public void Reduce_OnRightInstance_ShouldApplyFunction()
    {
        var right = new Either.Right<int, string>("test");

        var result = right.Reduce(s => s.Length);

        Assert.Equal(4, result);
    }

    [Fact]
    public void MapToUpper_OnRightInstance_ShouldApplyFunction()
    {
        var right = new Either.Right<int, string>("test");

        var result = right.Map(s => s.ToUpper());

        Assert.IsType<Either.Right<int, string>>(result);
        Assert.Equal("TEST", result.Match(x => "failure", x => x));
    }

    [Fact]
    public void Reduce_OnLeft_ShouldReturnLeft()
    {
        var first = new Either.Left<string, string>("test");
        var second = new Either.Left<int, int[]>(-1);

        Assert.Equal("test", first.Reduce(x => x.ToUpper()));
        Assert.Equal(-1, second.Reduce(r => r.Sum()));
    }

    [Fact]
    public void Match_OnLeft_ShouldNotApplyFunction()
    {
        Assert.Equal("test",
            new Either.Left<string, string>("test")
            .Map(s => s.ToUpper())
            .Match(f => f, f => f));
    }

    [Fact]
    public void Match_OnRight_ShouldApplyFunction()
    {
        Assert.Equal(10, 
            new Either.Right<int, int>(5)
                .Map(i => i * 2)
                .Match(l => l, r => r));
    }

    [Fact]
    public void ReduceOnRight_ShouldReturnReduce()
    {
        var first = new Either.Right<string, string>("test");
        var second = new Either.Right<int, int[]>([1, 2, 3, 4, 5]);

        Assert.Equal("TEST", first.Reduce(x => x.ToUpper()));
        Assert.Equal(15, second.Reduce(r => r.Sum()));
    }

    [Fact]
    public void Swap_OnRight_ShouldReduceRightValue()
    {
        var right = new Either.Right<int, string>("test");

        Assert.Equal("TEST", right.Map(s => s.ToUpper()).Swap().Reduce(x => "should not match"));
    }

    [Fact]
    public void Swap_OnLeft_ShouldReduceOriginalLeft()
    {
        Assert.Equal(2,
            new Either.Left<Exception[], int>([new ArgumentException(), new ArithmeticException()])
                .Swap()
                .Reduce(f => f.Length));
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

    private static Either<L, R> GetLeft<L, R>(L val) => val;
    private static Either<L, R> GetRight<L, R>(R val) => val;

    [Fact]
    public void Comparison_WithDifferentData_ShouldNotBeEqual()
    {
        var left1 = Either<int, string>.Left(2);
        var left2 = Either<int, string>.Left(3);
        var right1 = Either<int, string>.Right("test");
        var right2 = Either<int, string>.Right("testtest");

        Assert.NotEqual(left1, left2);
        Assert.NotEqual(right1, right2);
        Assert.NotEqual(left1, right2);
    }

    [Fact]
    public void IfLeft_OnLeft_ShouldReturnProvidedData()
    {
        string ReturnTest() => "test";
        Assert.Equal(4, Either<Exception, int>.Left(new Exception()).IfLeft(4));
        Assert.Equal("test", Either<int, string>.Left(4).IfLeft(ReturnTest)); 
    }

    [Fact]
    public void IfLeft_OnRight_ShouldReturnRightData()
    {
        int ReturnRandom() => 69;
        Assert.Equal("test", new Either.Right<Exception, string>("TEST").Map(x => x.ToLower()).IfLeft("WRONGVALUE"));
        Assert.Equal(32, new Either.Right<string, int>(32).IfLeft(ReturnRandom));
    }
}