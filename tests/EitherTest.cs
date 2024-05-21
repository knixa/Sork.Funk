using Sork.Funk;

namespace Sork.Funk.Tests;

public class EitherTest
{
    [Fact]
    public void Map_OnRightInstance_ShouldApplyFunction()
    {
        Either<string, int>.Right(10)
            .Map(x => x * 2.0)
            .Match(l => -0, r => r)
            .Should().Be(20);
    }

    [Fact]
    public void Map_OnLeftInstance_ShouldNotApplyFunction()
    {
        Either<int, string>.Left(10)
            .Map(s => s.ToUpper())
            .Reduce(s => -100)
            .Should().Be(10);
    }

    [Fact]
    public void Reduce_OnRightInstance_ShouldApplyFunction()
    {
        Either<int, string>.Right("test")
            .Reduce(s => s.Length)
            .Should().Be(4);
    }

    [Fact]
    public void MapToUpper_OnRightInstance_ShouldApplyFunction()
    {
        Either<int, string>.Right("test")
            .Map(s => s.ToUpper())
            .Match(l => "failure", r => r)
            .Should().Be("TEST");
    }

    [Fact]
    public void Reduce_OnLeft_ShouldReturnLeft()
    {
        Either<string, string>.Left("test")
            .Reduce(s => s.ToUpper())
            .Should().Be("test");

        Either<int, int[]>.Left(-10)
            .Reduce(r => r.Sum())
            .Should().Be(-10);
    }

    [Fact]
    public void Match_OnLeft_ShouldNotApplyFunction()
    {
        Either<string, string>.Left("test")
            .Map(s => s.ToUpper())
            .Match(f => f, f => f)
            .Should().Be("test");
    }

    [Fact]
    public void Match_OnRight_ShouldApplyFunction()
    {
        new Either.Right<int, int>(5)
            .Map(i => i * 2)
            .Match(l => l, r => r).Should().Be(10);
    }

    [Fact]
    public void ReduceOnRight_ShouldReturnReduce()
    {
        new Either.Right<string, string>("test").Reduce(s => s.ToUpper()).Should().Be("TEST");
        new Either.Right<int, int[]>([1, 2, 3, 4, 5]).Reduce(r => r.Sum()).Should().Be(15);

    }

    [Fact]
    public void Swap_OnRight_ShouldReduceRightValue()
    {
        Either<int, string>.Right("test")
            .Map(s => s.ToUpper())
            .Swap()
            .Reduce(x => "should not match")
            .Should().Be("TEST");
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
        right1.IsRight.Should().BeTrue();
        left1.IsLeft.Should().BeTrue();
        right2.IsLeft.Should().BeFalse();
        left2.IsRight.Should().BeFalse();
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

    [Fact]
    public void BiMap_OnLeft_ShouldReturnNewLeftData()
    {
        Assert.Equal(4, Either<Exception, string>.Left(new Exception())
            .BiMap(l => 4, r => 7.0).Reduce(x => (int)x));
    }

    [Fact]
    public void BiMap_OnRight_ShouldReduceNewRightData()
    {
        Assert.Equal("TEST",
            Either<Exception, double>.Right(4.0)
                .BiMap(l => l.Message, r => "test").Reduce(f => f.ToUpper()));
    }
    
}