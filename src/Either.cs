namespace Sork.Funk;

public abstract class Either<TLeft, TRight>
{

    public abstract Either<TNewLeft, TRight> MapLeft<TNewLeft>(Func<TLeft, TNewLeft> map);
    public abstract Either<TLeft, TNewRight> MapRight<TNewRight>(Func<TRight, TNewRight> map);
    public abstract TLeft Reduce(Func<TRight, TLeft> map);

    public abstract Either<TRight, TLeft> Swap();
    public abstract TResult Fold<TResult>(Func<TLeft, TResult> left, Func<TRight, TResult> right);
}

public sealed class Left<TLeft, TRight> : Either<TLeft, TRight>
{
    private readonly TLeft _value;

    public Left(TLeft value)
    {
        _value = value;
    }

    public override Either<TNewLeft, TRight> MapLeft<TNewLeft>(Func<TLeft, TNewLeft> map) =>
        new Left<TNewLeft, TRight>(map(_value));

    public override Either<TLeft, TNewRight> MapRight<TNewRight>(Func<TRight, TNewRight> map) =>
        new Left<TLeft, TNewRight>(_value);
    
    public override TLeft Reduce(Func<TRight, TLeft> map) => _value;
    public override Either<TRight, TLeft> Swap() => new Right<TRight, TLeft>(_value);
    public override TResult Fold<TResult>(Func<TLeft, TResult> left, Func<TRight, TResult> right)
    {
        return left(_value);
    }
}

public sealed class Right<TLeft, TRight> : Either<TLeft, TRight>
{
    private readonly TRight _value;

    public Right(TRight value)
    {
        _value = value;
    }

    public override Either<TNewLeft, TRight> MapLeft<TNewLeft>(Func<TLeft, TNewLeft> map) =>
        new Right<TNewLeft, TRight>(_value);

    public override Either<TLeft, TNewRight> MapRight<TNewRight>(Func<TRight, TNewRight> map) =>
        new Right<TLeft, TNewRight>(map(_value));

    public override TLeft Reduce(Func<TRight, TLeft> map) => map(_value);
    public override Either<TRight, TLeft> Swap() => new Left<TRight, TLeft>(_value);
    public override TResult Fold<TResult>(Func<TLeft, TResult> fold, Func<TRight, TResult> right)
    {
        return right(_value);
    }
}