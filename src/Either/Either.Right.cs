using System.Diagnostics.Contracts;

namespace Sork.Funk;

public static partial class Either
{
    public sealed record Right<TL, TR> : Either<TL, TR>
    {
        private readonly TR _value;

        public Right(TR value) => _value = value;

        public override bool IsLeft => false;
        public override bool IsRight => true;

        public override Either<TL, TNewR> Map<TNewR>(Func<TR, TNewR> map) =>
            new Right<TL, TNewR>(map(_value));

        public override TR IfLeft(Func<TR> map) => _value;

        public override TR IfLeft(TR value) => _value;

        [Pure]
        public override TL Reduce(Func<TR, TL> map) => map(_value);

        [Pure]
        public override Either<TR, TL> Swap() => new Left<TR, TL>(_value);

        [Pure]
        public override TResult Match<TResult>(Func<TL, TResult> left, Func<TR, TResult> right) => right(_value);

        public override void Match(Action<TL> left, Action<TR> right) => right(_value);
    }
}