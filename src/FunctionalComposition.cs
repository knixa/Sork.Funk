namespace Sork.Funk;

public static partial class Composition
{
    public static Func<T, TResult> Compose<T, TIntermediate, TResult>(
        this Func<T, TIntermediate> inner,
        Func<TIntermediate, TResult> outer) =>
        x => outer(inner(x));
}
