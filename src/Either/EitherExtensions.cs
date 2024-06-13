using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Sork.Funk;

public static class EitherExtensions
{
    /// <summary>
    /// Unwraps the left values from a collection of Either objects.
    /// </summary>
    /// <typeparam name="TL">The type of the left value.</typeparam>
    /// <typeparam name="TR">The type of the right value.</typeparam>
    /// <param name="collection">The collection of Either objects.</param>
    /// <returns>An IEnumerable of left values where the Either object is a Left.</returns>
    [Pure]
    public static IEnumerable<TL> UnwrapLeft<TL, TR>(this IEnumerable<Either<TL, TR>> collection) =>
        collection.Where(e => e.IsLeft).Select(l => l.LeftValue);

    /// <summary>
    /// Unwraps the right values from a collection of Either objects.
    /// </summary>
    /// <typeparam name="TL">The type of the left value.</typeparam>
    /// <typeparam name="TR">The type of the right value.</typeparam>
    /// <param name="collection">The collection of Either objects.</param>
    /// <returns>An IEnumerable of right values where the Either object is a Right.</returns>
    [Pure]
    public static IEnumerable<TR> UnwrapRight<TL, TR>(this IEnumerable<Either<TL, TR>> collection) =>
        collection.Where(e => e.IsRight).Select(r => r.RightValue);
}
