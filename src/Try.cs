using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Sork.Funk;

public sealed class Try<T>
{
    private readonly T _value;
    private readonly Exception? _exception;

    private Try(Exception? exception)
    {
        _exception = exception ?? new ArgumentNullException(nameof(exception), "Exception cannot be null.");
        _value = default!;
    }

    private Try(T value)
    {
        _value = value;
        _exception = null;
    }

    [MemberNotNullWhen(false, nameof(_exception))]
    private bool IsSuccess => _exception is null;

    /// <summary>
    /// Creates a successful instance of <see cref="Try{T}"/> containing a value.
    /// </summary>
    /// <param name="value">The value to wrap in the successful instance.</param>
    /// <returns>A successful <see cref="Try{T}"/> containing the specified value.</returns>
    [Pure]
    public static Try<T> Success(T value) => new(value);

    /// <summary>
    /// Creates a failed instance of <see cref="Try{T}"/> containing an exception.
    /// </summary>
    /// <param name="exception">The exception to wrap in the failed instance.</param>
    /// <returns>A failed <see cref="Try{T}"/> containing the specified exception.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="exception"/> is null.</exception>
    [Pure]
    public static Try<T> Failure(Exception exception) => new(exception);

    /// <summary>
    /// Matches the state of the <see cref="Try{T}"/> instance and invokes the appropriate function.
    /// </summary>
    /// <typeparam name="TResult">The type of the result produced by the matching function.</typeparam>
    /// <param name="onSuccess">Function to invoke if the instance is successful.</param>
    /// <param name="onFailure">Function to invoke if the instance is a failure.</param>
    /// <returns>
    /// The result of the <paramref name="onSuccess"/> function if the instance is successful,
    /// or the result of the <paramref name="onFailure"/> function if the instance is a failure.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if either <paramref name="onSuccess"/> or <paramref name="onFailure"/> is null.
    /// </exception>
    [Pure]
    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Exception, TResult> onFailure) =>
        IsSuccess ? onSuccess(_value) : onFailure(_exception);

    /// <summary>
    /// Applies a transformation function to the value of a successful <see cref="Try{T}"/> instance,
    /// or propagates the failure if the instance is unsuccessful.
    /// </summary>
    /// <typeparam name="TResult">The type of the resulting value after the transformation.</typeparam>
    /// <param name="mapper">A function to transform the value of the successful instance.</param>
    /// <returns>
    /// A new <see cref="Try{T}"/> containing the transformed value if the instance is successful,
    /// or the propagated failure if the instance is unsuccessful.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="mapper"/> is null.</exception>
    /// <exception cref="Exception">Wrapped if thrown during the execution of <paramref name="mapper"/>.</exception>
    public Try<TResult> Map<TResult>(Func<T, TResult> mapper)
    {
        if (!IsSuccess)
        {
            return new Try<TResult>(_exception);
        }

        try
        {
            return new Try<TResult>(mapper(_value));
        }
        catch (Exception ex)
        {
            return new Try<TResult>(ex);
        }
    }

    /// <summary>
    /// Maps the value of a successful <see cref="Try{T}"/> instance to a new asynchronous operation.
    /// </summary>
    /// <typeparam name="TResult">The type of the resulting value.</typeparam>
    /// <param name="mapper">
    /// A function that transforms the value of the successful instance asynchronously.
    /// Consumers can optionally pass a <see cref="CancellationToken"/> via closure.
    /// </param>
    /// <returns>
    /// A new <see cref="Try{T}"/> containing the transformed value if the instance is successful.
    /// If the asynchronous operation throws an exception, it is wrapped in a failure.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// Rethrown if the asynchronous operation is canceled.
    /// </exception>
    public async Task<Try<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> mapper)
    {
        if (!IsSuccess)
        {
            return new Try<TResult>(_exception);
        }

        try
        {
            return new Try<TResult>(await mapper(_value).ConfigureAwait(false));
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return new Try<TResult>(ex);
        }
    }

    /// <summary>
    /// Returns the value wrapped in the <see cref="Try{T}"/> if the instance represents success;
    /// otherwise, returns the specified default value.
    /// </summary>
    /// <param name="defaultValue">The value to return if the instance represents failure.</param>
    /// <returns>
    /// The wrapped value if the instance represents success; otherwise, the specified default value.
    /// </returns>
    /// <remarks>
    /// This method is convenient for cases where a meaningful default value can be provided.
    /// However, frequent use may indicate misuse of the <see cref="Try{T}"/> abstraction,
    /// as it bypasses explicit handling of failures.
    /// </remarks>
    [Pure]
    public T OrDefault(T defaultValue) => IsSuccess ? _value : defaultValue;

    /// <summary>
    /// Returns the value wrapped in the <see cref="Try{T}"/> if the instance represents success;
    /// otherwise, evaluates the provided function to obtain a default value.
    /// </summary>
    /// <param name="defaultProvider">
    /// A function to lazily provide the default value if the instance represents failure.
    /// </param>
    /// <returns>
    /// The wrapped value if the instance represents success; otherwise, the result of the <paramref name="defaultProvider"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="defaultProvider"/> is null.
    /// </exception>
    /// <remarks>
    /// Use this method to provide a dynamically calculated default value, particularly
    /// in cases where the computation might be expensive or context-dependent.
    /// Frequent reliance on this method might suggest an antipattern and could bypass
    /// intentional failure handling.
    /// </remarks>
    [Pure]
    public T OrDefault(Func<T> defaultProvider)
    {
        ArgumentNullException.ThrowIfNull(defaultProvider);
        return IsSuccess ? _value : defaultProvider();
    }
}
