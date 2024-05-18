namespace Sork.Funk;

public abstract record Either<TL, TR>
{

    public abstract Either<TNewL, TR> MapLeft<TNewL>(Func<TL, TNewL> map);
    public abstract Either<TL, TNewR> MapRight<TNewR>(Func<TR, TNewR> map);
    public abstract TL Reduce(Func<TR, TL> map);

    public abstract Either<TR, TL> Swap();
    public abstract TResult Match<TResult>(Func<TL, TResult> left, Func<TR, TResult> right);
}

public sealed record Left<TL, TR> : Either<TL, TR>
{
    private readonly TL _value;

    public Left(TL value)
    {
        _value = value;
    }

    public override Either<TNewL, TR> MapLeft<TNewL>(Func<TL, TNewL> map) =>
        new Left<TNewL, TR>(map(_value));

    public override Either<TL, TNewR> MapRight<TNewR>(Func<TR, TNewR> map) =>
        new Left<TL, TNewR>(_value);
    
    public override TL Reduce(Func<TR, TL> map) => _value;
    public override Either<TR, TL> Swap() => new Right<TR, TL>(_value);
    public override TResult Match<TResult>(Func<TL, TResult> left, Func<TR, TResult> right)
    {
        return left(_value);
    }
}

public sealed record Right<TL, TR> : Either<TL, TR>
{
    private readonly TR _value;

    public Right(TR value)
    {
        _value = value;
    }

    public override Either<TNewL, TR> MapLeft<TNewL>(Func<TL, TNewL> map) =>
        new Right<TNewL, TR>(_value);

    public override Either<TL, TNewR> MapRight<TNewR>(Func<TR, TNewR> map) =>
        new Right<TL, TNewR>(map(_value));

    public override TL Reduce(Func<TR, TL> map) => map(_value);
    public override Either<TR, TL> Swap() => new Left<TR, TL>(_value);
    public override TResult Match<TResult>(Func<TL, TResult> fold, Func<TR, TResult> right)
    {
        return right(_value);
    }
}