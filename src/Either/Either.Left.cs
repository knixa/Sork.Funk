using System.Diagnostics.Contracts;

namespace Sork.Funk;

public static partial class Either
{
    public sealed record Left<TL, TR>(TL Value) : Either<TL, TR>
    {
        public override bool IsLeft => true;
        public override bool IsRight => false;

        public override Either<TL, TNewR> Map<TNewR>(Func<TR, TNewR> map) => new Left<TL, TNewR>(Value);
        public override TR IfLeft(Func<TR> map) => map();

        public override TR IfLeft(TR value) => value;
        public override TL IfRight(Func<TL> map) => Value;
        public override TL IfRight(TL value) => Value;

        [Pure]
        public override TL Reduce(Func<TR, TL> map) => Value;

        [Pure]
        public override Either<TR, TL> Swap() => new Right<TR, TL>(Value);

        [Pure]
        public override TResult Match<TResult>(Func<TL, TResult> left, Func<TR, TResult> right) => left(Value);

        [Pure]
        public override Either<TNewL, TNewR> BiMap<TNewL, TNewR>(Func<TL, TNewL> left, Func<TR, TNewR> right) =>
            new Left<TNewL, TNewR>(left(Value));
    }
}