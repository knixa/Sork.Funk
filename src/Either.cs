using System.Diagnostics.Contracts;

namespace Sork.Funk;

public abstract record Either<TL, TR>
{
    public abstract bool IsLeft { get; }
    public abstract bool IsRight { get; }

    public abstract Either<TL, TNewR> Map<TNewR>(Func<TR, TNewR> map);

    [Pure]
    public abstract TL Reduce(Func<TR, TL> map);

    [Pure]
    public abstract Either<TR, TL> Swap();

    [Pure]
    public abstract TResult Match<TResult>(Func<TL, TResult> left, Func<TR, TResult> right);

    public abstract void Match(Action<TL> left, Action<TR> right);
    
    [Pure]
    public static implicit operator Either<TL, TR>(TL value) => new Left<TL, TR>(value);

    [Pure]
    public static implicit operator Either<TL, TR>(TR value) => new Right<TL, TR>(value);
}

public sealed record Left<TL, TR> : Either<TL, TR>
{
    private readonly TL _value;

    public Left(TL value) => _value = value;

    public override bool IsLeft => true;
    public override bool IsRight => false;

    public override Either<TL, TNewR> Map<TNewR>(Func<TR, TNewR> map) => new Left<TL, TNewR>(_value);

    [Pure]
    public override TL Reduce(Func<TR, TL> map) => _value;

    [Pure]
    public override Either<TR, TL> Swap() => new Right<TR, TL>(_value);

    [Pure]
    public override TResult Match<TResult>(Func<TL, TResult> left, Func<TR, TResult> right) => left(_value);

    public override void Match(Action<TL> left, Action<TR> right) => left(_value);
}

public sealed record Right<TL, TR> : Either<TL, TR>
{
    private readonly TR _value;

    public Right(TR value) => _value = value;

    public override bool IsLeft => false;
    public override bool IsRight => true;

    public override Either<TL, TNewR> Map<TNewR>(Func<TR, TNewR> map) =>
        new Right<TL, TNewR>(map(_value));

    [Pure]
    public override TL Reduce(Func<TR, TL> map) => map(_value);

    [Pure]
    public override Either<TR, TL> Swap() => new Left<TR, TL>(_value);

    [Pure]
    public override TResult Match<TResult>(Func<TL, TResult> left, Func<TR, TResult> right) => right(_value);

    public override void Match(Action<TL> left, Action<TR> right) => right(_value);
}