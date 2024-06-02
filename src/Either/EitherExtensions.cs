namespace Sork.Funk;

public static class EitherExtensions
{
    public static IEnumerable<TL> UnwrapLeft<TL, TR>(this IEnumerable<Either<TL, TR>> collection) => 
        collection.Where(e => e.IsLeft).Select(l => l.LeftValue);

    public static IEnumerable<TR> UnwrapRight<TL, TR>(this IEnumerable<Either<TL, TR>> collection) =>
        collection.Where(e => e.IsRight).Select(r => r.RightValue);
}