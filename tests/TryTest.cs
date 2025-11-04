namespace Sork.Funk.Tests;

public class TryTest
{
    [Fact]
    public void Success_ShouldCreateSuccessfulTry()
    {
        var value = 42;

        var trySuccess = Try<int>.Success(value);

        Assert.True(trySuccess.Match(
            onSuccess: _ => true,
            onFailure: _ => false));
    }

    [Fact]
    public void Failure_ShouldCreateFailedTry()
    {
        var exception = new InvalidOperationException("Test exception");

        var tryFailure = Try<int>.Failure(exception);

        Assert.False(tryFailure.Match(
            onSuccess: _ => true,
            onFailure: _ => false));
        Assert.True(tryFailure.Match(
            onSuccess: _ => false,
            onFailure: ex => ex == exception));
    }

    [Fact]
    public void Match_ShouldInvokeOnSuccess_WhenTryIsSuccessful()
    {
        var trySuccess = Try<int>.Success(10);

        var result = trySuccess.Match(
            onSuccess: x => x * 2,
            onFailure: _ => -1);

        Assert.Equal(20, result);
    }

    [Fact]
    public void Match_ShouldInvokeOnFailure_WhenTryIsFailure()
    {
        var exception = new InvalidOperationException("Test failure");
        var tryFailure = Try<int>.Failure(exception);

        var result = tryFailure.Match(
            onSuccess: _ => 100,
            onFailure: ex => ex.Message.Length);

        Assert.Equal(exception.Message.Length, result);
    }

    [Fact]
    public void Map_ShouldApplyMapper_WhenTryIsSuccessful()
    {
        var trySuccess = Try<int>.Success(5);

        var mappedTry = trySuccess.Map(x => x * 2);

        Assert.True(mappedTry.Match(
            onSuccess: result => result == 10,
            onFailure: _ => false));
    }

    [Fact]
    public void Map_ShouldReturnFailure_WhenTryIsFailure()
    {
        var exception = new InvalidOperationException("Test exception");
        var tryFailure = Try<int>.Failure(exception);

        var mappedTry = tryFailure.Map(x => x * 2);

        Assert.True(mappedTry.Match(
            onSuccess: _ => false,
            onFailure: ex => ex == exception));
    }

    [Fact]
    public void Map_ShouldCatchMapperExceptionsAndReturnFailure()
    {
        var trySuccess = Try<int>.Success(10);

        var mappedTry = trySuccess.Map<int>(x => throw new ArgumentException("Mapping failed"));

        Assert.True(mappedTry.Match(
            onSuccess: _ => false,
            onFailure: ex => ex is ArgumentException));
    }

    [Fact]
    public async Task MapAsync_ShouldReturnFailure_WhenTryIsFailure()
    {
        var exception = new InvalidOperationException("Test exception");
        var tryFailure = Try<int>.Failure(exception);

        var mappedTry = await tryFailure.MapAsync(async x => x * 2);

        Assert.True(mappedTry.Match(
            onSuccess: _ => false,
            onFailure: ex => ex == exception));
    }

    [Fact]
    public async Task MapAsync_ShouldCatchMapperExceptionsAndReturnFailure()
    {
        var trySuccess = Try<int>.Success(10);

        var mappedTry = await trySuccess.MapAsync<int>(async (x) => throw new ArgumentException("Mapping failed"));

        Assert.True(mappedTry.Match(
            onSuccess: _ => false,
            onFailure: ex => ex is ArgumentException));
    }

    [Fact]
    public async Task MapAsync_ShouldPropagateException_WhenTaskIsCanceled()
    {
        var trySuccess = Try<int>.Success(10);

        var tokenSource = new CancellationTokenSource();

        Func<int, CancellationToken, Task<int>> squareAsync = (x, ct) =>
        {
            ct.ThrowIfCancellationRequested();
            return Task.FromResult(x * x);
        };

        await tokenSource.CancelAsync();
        var toTest = trySuccess.MapAsync((x) => squareAsync(x, tokenSource.Token));


        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await toTest);
    }

    [Fact]
    public void OrDefault_ShouldReturnValue_WhenTryIsSuccessful()
    {
        var trySuccess = Try<int>.Success(42);

        var result = trySuccess.OrDefault(99);

        Assert.Equal(42, result);
    }

    [Fact]
    public void OrDefault_ShouldReturnDefault_WhenTryIsFailure()
    {
        var tryFailure = Try<int>.Failure(new InvalidOperationException("Test exception"));

        var result = tryFailure.OrDefault(99);

        Assert.Equal(99, result);
    }

    [Fact]
    public void OrDefault_WithProvider_ShouldReturnValue_WhenTryIsSuccessful()
    {
        var trySuccess = Try<int>.Success(42);

        var result = trySuccess.OrDefault(() => 99);

        Assert.Equal(42, result);
    }

    [Fact]
    public void OrDefault_WithProvider_ShouldInvokeProvider_WhenTryIsFailure()
    {
        var tryFailure = Try<int>.Failure(new InvalidOperationException("Test exception"));
        var defaultProviderInvoked = false;

        var result = tryFailure.OrDefault(() =>
        {
            defaultProviderInvoked = true;
            return 99;
        });

        Assert.True(defaultProviderInvoked);
        Assert.Equal(99, result); 
    }

    [Fact]
    public void OrDefault_WithProvider_ShouldThrow_WhenProviderIsNull()
    {
        var tryFailure = Try<int>.Failure(new InvalidOperationException("Test exception"));

        Assert.Throws<ArgumentNullException>(() => tryFailure.OrDefault(null!));
    }
}
