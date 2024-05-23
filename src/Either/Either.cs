using System.Diagnostics.Contracts;

namespace Sork.Funk;

public abstract record Either<TL, TR>
{
    public abstract bool IsLeft { get; }
    public abstract bool IsRight { get; }

    [Pure]
    public static Either<TL, TR> Left(TL value) => new Either.Left<TL, TR>(value);

    [Pure]
    public static Either<TL, TR> Right(TR value) => new Either.Right<TL, TR>(value);

    public abstract Either<TL, TNewR> Map<TNewR>(Func<TR, TNewR> map);

    [Pure]
    public abstract TR IfLeft(Func<TR> map);

    [Pure]
    public abstract TR IfLeft(TR value);

    [Pure]
    public abstract TL IfRight(Func<TL> map);

    [Pure]
    public abstract TL IfRight(TL value);

    [Pure]
    public abstract TL Reduce(Func<TR, TL> map);

    [Pure]
    public abstract Either<TR, TL> Swap();

    [Pure]
    public abstract TResult Match<TResult>(Func<TL, TResult> left, Func<TR, TResult> right);

    [Pure]
    public static implicit operator Either<TL, TR>(TL value) => new Either.Left<TL, TR>(value);

    [Pure]
    public static implicit operator Either<TL, TR>(TR value) => new Either.Right<TL, TR>(value);

    [Pure]
    public abstract Either<TNewL, TNewR> BiMap<TNewL, TNewR>(Func<TL, TNewL> left, Func<TR, TNewR> right);
}