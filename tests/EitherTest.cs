using FsCheck;
using FsCheck.Xunit;
using Random = System.Random;

namespace Sork.Funk.Tests;

public class EitherTest
{
    [Fact]
    public void Map_OnRightInstance_ShouldApplyFunction() =>
        Assert.Equal(20, Either<string, int>.Right(10)
            .Map(x => x * 2.0)
            .Match(l => -0, r => r));

    [Fact]
    public void Map_OnLeftInstance_ShouldNotApplyFunction() =>
        Assert.Equal(10,Either<int, string>.Left(10)
            .Map(s => s.ToUpper())
            .Reduce(s => -100));


    [Fact]
    public void Reduce_OnRightInstance_ShouldApplyFunction() =>
        Assert.Equal(4, Either<int, string>.Right("test")
            .Reduce(s => s.Length));

    [Fact]
    public void MapToUpper_OnRightInstance_ShouldApplyFunction() =>
        Assert.Equal("TEST", Either<int, string>.Right("test")
            .Map(s => s.ToUpper())
            .Match(l => "failure", r => r));

    [Fact]
    public void Reduce_OnLeft_ShouldReturnLeft()
    {
        Assert.Equal("test", Either<string, string>.Left("test")
            .Reduce(s => s.ToUpper()));

        Assert.Equal(-10, Either<int, int[]>.Left(-10)
            .Reduce(r => r.Sum()));
    }

    [Fact]
    public void Match_OnLeft_ShouldNotApplyFunction() =>
        Assert.Equal("test", Either<string, string>.Left("test")
            .Map(s => s.ToUpper())
            .Match(f => f, f => f));

    [Fact]
    public void Match_OnRight_ShouldApplyFunction() =>
        Assert.Equal(10,Either<int, int>.Right(5)
            .Map(i => i * 2)
            .Match(l => l, r => r));

    [Fact]
    public void ReduceOnRight_ShouldReturnReduce()
    {
        Assert.Equal("TEST",Either<string, string>.Right("test").Reduce(s => s.ToUpper()));
        Assert.Equal(15, Either<int, int[]>.Right([1, 2, 3, 4, 5]).Reduce(r => r.Sum()));
    }

    [Property]
    public void Swap_OnRight_ShouldReduceRightValue(NonNull<string> val) =>
        Assert.Equal(val.Get.ToUpper(), Either<int, string>.Right(val.Get)
            .Map(s => s.ToUpper())
            .Swap()
            .Reduce(x => "should not match"));

    [Fact]
    public void Swap_OnLeft_ShouldReduceOriginalLeft() =>
        Assert.Equal(2,
            Either<Exception[], int>.Left([new ArgumentException(), new ArithmeticException()])
                .Swap()
                .Reduce(f => f.Length));

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
        Assert.True(right1.IsRight);
        Assert.True(left1.IsLeft);
        Assert.False(right2.IsLeft);
        Assert.False(left2.IsRight);
    }

    [Fact]
    public void IfLeft_OnLeft_ShouldReturnProvidedData()
    {
        string ReturnTest()
        {
            return "test";
        }

        Assert.Equal(4, Either<Exception, int>.Left(new Exception()).IfLeft(4));
        Assert.Equal("test", Either<int, string>.Left(4).IfLeft(ReturnTest));
    }

    [Fact]
    public void IfLeft_OnRight_ShouldReturnRightData()
    {
        int ReturnRandom()
        {
            return 69;
        }

        Assert.Equal("test", Either<Exception, string>.Right("TEST").Map(x => x.ToLower()).IfLeft("WRONGVALUE"));
        Assert.Equal(32, Either<string, int>.Right(32).IfLeft(ReturnRandom));
    }

    [Property]
    public void BiMap_OnLeft_ShouldReturnNewLeftData(int num) =>
        Assert.Equal(num, Either<Exception, string>.Left(new Exception())
            .BiMap(l => num, r => 7.0).Reduce(x => (int)x));

    [Property]
    public void BiMap_OnRight_ShouldReduceNewRightData(NonNull<string> val) =>
        Assert.Equal(val.Get.ToUpper(),
            Either<Exception, double>.Right(4.0)
                .BiMap(l => l.Message, r => val.Get).Reduce(f => f.ToUpper()));

    [Property]
    public void IfRight_OnRight_ShouldReturnProvidedData(int num)
    {
        var data = new Random();
        Assert.Equal(data, Either<Random, int>.Right(num).IfRight(data));
    }

    [Property]
    public void IfRight_OnRight_ShouldReturnProvidedFunctionData(int num)
    {
        var data = new Random();

        Random DataFunc()
        {
            return data;
        }

        Assert.Equal(data, Either<Random, int>.Right(num).IfRight(DataFunc));
    }
}
