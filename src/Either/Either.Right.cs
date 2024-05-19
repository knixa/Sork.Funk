using System.Diagnostics.Contracts;

namespace Sork.Funk;

public static partial class Either
{
    public sealed record Right<TL, TR>(TR Value) : Either<TL, TR>
    {
        public override bool IsLeft => false;
        public override bool IsRight => true;

        public override Either<TL, TNewR> Map<TNewR>(Func<TR, TNewR> map) =>
            new Right<TL, TNewR>(map(Value));

        public override TR IfLeft(Func<TR> map) => Value;

        public override TR IfLeft(TR value) => Value;

        [Pure]
        public override TL Reduce(Func<TR, TL> map) => map(Value);

        [Pure]
        public override Either<TR, TL> Swap() => new Left<TR, TL>(Value);

        [Pure]
        public override TResult Match<TResult>(Func<TL, TResult> left, Func<TR, TResult> right) => right(Value);

        public override void Match(Action<TL> left, Action<TR> right) => right(Value);

        [Pure]
        public override Either<TNewL, TNewR> BiMap<TNewL, TNewR>(Func<TL, TNewL> left, Func<TR, TNewR> right) =>
            new Right<TNewL, TNewR>(right(Value));
    }
}