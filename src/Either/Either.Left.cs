using System.Diagnostics.Contracts;

namespace Sork.Funk;

public static partial class Either
{

    public sealed record Left<TL, TR> : Either<TL, TR>
    {
        private readonly TL _value;

        public Left(TL value) => _value = value;

        public override bool IsLeft => true;
        public override bool IsRight => false;

        public override Either<TL, TNewR> Map<TNewR>(Func<TR, TNewR> map) => new Left<TL, TNewR>(_value);
        public override TR IfLeft(Func<TR> map) => map();

        public override TR IfLeft(TR value) => value;

        [Pure]
        public override TL Reduce(Func<TR, TL> map) => _value;

        [Pure]
        public override Either<TR, TL> Swap() => new Right<TR, TL>(_value);

        [Pure]
        public override TResult Match<TResult>(Func<TL, TResult> left, Func<TR, TResult> right) => left(_value);

        public override void Match(Action<TL> left, Action<TR> right) => left(_value);
    }
}