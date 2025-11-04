namespace Sork.Funk.Tests.Either;

public class EitherExtensionsTest
{
    [Fact]
    public void UnwrapLeft_WithLeftValues_ShouldReturnLeft() =>
        Assert.Equal(2,
            new[] { Either<int, string>.Left(4), Either<int, string>.Left(2), Either<int, string>.Right("test") }
                .UnwrapLeft().Count());

    [Fact]
    public void UnwrapLeft_WithRight_ShouldBeEmpty() =>
        Assert.Empty(new[] { Either<Exception, int>.Right(4), Either<Exception, int>.Right(10) }.UnwrapLeft());

    [Fact]
    public void UnwrapRight_WithRight_ShouldReturnRight() =>
        Assert.Equal(2,
            new[] { Either<string, int>.Right(9), Either<string, int>.Right(3), Either<string, int>.Left("test") }
                .UnwrapRight().Count());

    [Fact]
    public void UnwrapRight_WithLeft_ShouldBeEmpty() =>
        Assert.Empty(new[]
        {
            Either<Exception, string>.Left(new Exception()), Either<Exception, string>.Left(new Exception()),
            Either<Exception, string>.Left(new Exception())
        }.UnwrapRight());
}
