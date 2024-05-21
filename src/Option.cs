using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Sork.Funk;

public readonly record struct Option<T> where T : notnull
{
    private readonly T? _value;

    [Pure] public bool IsSome { get; }

    [Pure] public bool IsNone => !IsSome;

    private Option(T value)
    {
        _value = value;
        IsSome = true;
    }

    /// <summary>
    /// Creates an <see cref="Option{T}"/> with a non-null value.
    /// </summary>
    /// <param name="value">The non-null value to wrap in an option.</param>
    /// <returns>An option containing the specified value.</returns>
    public static Option<T> Some([DisallowNull] T value)
    {
        return new Option<T>(value);
    }

    public static readonly Option<T> None = default;

    /// <summary>
    /// Maps the value of this option to a new value of type <typeparamref name="TNew"/>.
    /// </summary>
    /// <typeparam name="TNew">The type of the new value.</typeparam>
    /// <param name="map">A function that transforms the current value.</param>
    /// <returns>An option containing the mapped value.</returns>
    /// <remarks>If this option is None, the result will also be None.</remarks>
    [Pure]
    public Option<TNew> Map<TNew>(Func<T, TNew> map) where TNew : notnull =>
        IsSome
            ? Option<TNew>.Some(map(_value!))
            : default;

    /// <summary>
    /// Binds the current value to a new option of type <typeparamref name="TNew"/>.
    /// </summary>
    /// <typeparam name="TNew">The type of the new option's value.</typeparam>
    /// <param name="bind">A function that produces a new option based on the current value.</param>
    /// <returns>The resulting option after binding.</returns>
    /// <remarks>If this option is None, the result will also be None.</remarks>
    [Pure]
    public Option<TNew> Bind<TNew>(Func<T, Option<TNew>> bind) where TNew : notnull =>
        IsSome
            ? bind(_value!)
            : default;

    /// <summary>
    /// Matches the current option, invoking the appropriate delegate based on whether it is Some or None.
    /// </summary>
    /// <typeparam name="TRes">The type of the result.</typeparam>
    /// <param name="some">A function to apply to the value if this option is Some.</param>
    /// <param name="none">A function to apply if this option is None.</param>
    /// <returns>The result of applying the appropriate delegate.</returns>
    [Pure]
    public TRes Match<TRes>(Func<T, TRes> some, Func<TRes> none) =>
        IsSome
            ? some(_value!)
            : none();

    public Either<TL, T> ToEither<TL>(Func<TL> func) =>
        IsSome ? Either<TL, T>.Right(_value!) : Either<TL, T>.Left(func());
}