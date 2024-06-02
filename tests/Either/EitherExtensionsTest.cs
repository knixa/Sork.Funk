using Sork.Funk;

namespace Sork.Funk.Tests.Either;

public class EitherExtensionsTest
{
    [Fact]
    public void UnwrapLeft_WithLeftValues_ShouldReturnLeft() =>
        new[] { Either<int, string>.Left(4), Either<int, string>.Left(2), Either<int, string>.Right("test") }
            .UnwrapLeft()
            .Should().NotBeEmpty()
            .And.HaveCount(2);

    [Fact]
    public void UnwrapLeft_WithRight_ShouldBeEmpty() =>
        new[] { Either<Exception, int>.Right(4), Either<Exception, int>.Right(10) }.UnwrapLeft()
            .Should().BeEmpty();

    [Fact]
    public void UnwrapRight_WithRight_ShouldReturnRight() =>
        new[] { Either<string, int>.Right(9), Either<string, int>.Right(3), Either<string, int>.Left("test") }
            .UnwrapRight().Should().NotBeEmpty().And.HaveCount(2);

    [Fact]
    public void UnwrapRight_WithLeft_ShouldBeEmpty() =>
        new[]
        {
            Either<Exception, string>.Left(new Exception()), Either<Exception, string>.Left(new Exception()),
            Either<Exception, string>.Left(new Exception())
        }.UnwrapRight().Should().BeEmpty();
}
