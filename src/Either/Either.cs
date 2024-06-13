using System.Diagnostics.Contracts;

namespace Sork.Funk;

public abstract record Either<TL, TR>
{
    public abstract bool IsLeft { get; }
    public abstract bool IsRight { get; }

    /// <summary>
    /// Creates a new Either object with a Left value.
    /// </summary>
    /// <typeparam name="TL">The type of the left value.</typeparam>
    /// <typeparam name="TR">The type of the right value.</typeparam>
    /// <param name="value">The left value.</param>
    /// <returns>A new Either object with a Left value.</returns>
    [Pure]
    public static Either<TL, TR> Left(TL value) => new Either.Left<TL, TR>(value);

    /// <summary>
    /// Creates a new Either object with a Right value.
    /// </summary>
    /// <typeparam name="TL">The type of the left value.</typeparam>
    /// <typeparam name="TR">The type of the right value.</typeparam>
    /// <param name="value">The right value.</param>
    /// <returns>A new Either object with a Right value.</returns>
    [Pure]
    public static Either<TL, TR> Right(TR value) => new Either.Right<TL, TR>(value);

    /// <summary>
    /// Maps the Right value of the Either object to a new value.
    /// </summary>
    /// <typeparam name="TNewR">The type of the new right value.</typeparam>
    /// <param name="map">A function that maps the right value to a new value.</param>
    /// <returns>A new Either object with the mapped right value.</returns>
    public abstract Either<TL, TNewR> Map<TNewR>(Func<TR, TNewR> map);

    /// <summary>
    /// Returns the Right value if the Either is a Right, otherwise returns the result of the input function.
    /// </summary>
    /// <param name="map">A function that provides a value to return if the Either is a Left.</param>
    /// <returns>The Right value if the Either is a Right, otherwise the result of the input function.</returns>
    [Pure]
    public abstract TR IfLeft(Func<TR> map);

    /// <summary>
    /// Returns the Right value if the Either is a Right, otherwise returns the input value.
    /// </summary>
    /// <param name="value">The value to return if the Either is a Left.</param>
    /// <returns>The Right value if the Either is a Right, otherwise the input value.</returns>
    [Pure]
    public abstract TR IfLeft(TR value);

    /// <summary>
    /// Returns the Left value if the Either is a Left, otherwise returns the result of the input function.
    /// </summary>
    /// <param name="map">A function that provides a value to return if the Either is a Right.</param>
    /// <returns>The Left value if the Either is a Left, otherwise the result of the input function.</returns>
    [Pure]
    public abstract TL IfRight(Func<TL> map);

    /// <summary>
    /// Returns the Left value if the Either is a Left, otherwise returns the input value.
    /// </summary>
    /// <param name="value">The value to return if the Either is a Right.</param>
    /// <returns>The Left value if the Either is a Left, otherwise the input value.</returns>
    [Pure]
    public abstract TL IfRight(TL value);

    /// <summary>
    /// Reduces the Either to a Left value by mapping the Right value to a Left value.
    /// </summary>
    /// <param name="map">A function that maps the Right value to a Left value.</param>
    /// <returns>The Left value.</returns>
    [Pure]
    public abstract TL Reduce(Func<TR, TL> map);

    /// <summary>
    /// Swaps the Left and Right values of the Either.
    /// </summary>
    /// <returns>A new Either object with the Left and Right values swapped.</returns>
    [Pure]
    public abstract Either<TR, TL> Swap();

    /// <summary>
    /// Matches the Either to a result by applying a function to the Left or Right value.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="left">A function that maps the Left value to a result.</param>
    /// <param name="right">A function that maps the Right value to a result.</param>
    /// <returns>The result of applying the function to the Left or Right value.</returns>
    [Pure]
    public abstract TResult Match<TResult>(Func<TL, TResult> left, Func<TR, TResult> right);

    /// <summary>
    /// Implicitly converts a value to an Either with a Left value.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    [Pure]
    public static implicit operator Either<TL, TR>(TL value) => new Either.Left<TL, TR>(value);

    /// <summary>
    /// Implicitly converts a value to an Either with a Right value.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    [Pure]
    public static implicit operator Either<TL, TR>(TR value) => new Either.Right<TL, TR>(value);

    /// <summary>
    /// Maps both the Left and Right values of the Either to new values.
    /// </summary>
    /// <typeparam name="TNewL">The type of the new left value.</typeparam>
    /// <typeparam name="TNewR">The type of the new right value.</typeparam>
    /// <param name="left">A function that maps the Left value to a new value.</param>
    /// <param name="right">A function that maps the Right value to a new value.</param>
    /// <returns>A new Either object with the mapped Left and Right values.</returns>
    [Pure]
    public abstract Either<TNewL, TNewR> BiMap<TNewL, TNewR>(Func<TL, TNewL> left, Func<TR, TNewR> right);

    internal abstract TR RightValue { get; }
    internal abstract TL LeftValue { get; }
}
