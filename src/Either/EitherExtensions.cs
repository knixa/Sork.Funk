using System.Diagnostics.Contracts;

namespace Sork.Funk;

public static class EitherExtensions
{
    extension<L, R>(IEnumerable<Either<L, R>> collection)
    {
        /// <summary>
        /// Unwraps the left values from a collection of Either objects.
        /// </summary>
        /// <returns>An IEnumerable of left values where the Either object is a Left.</returns>
        [Pure]
        public IEnumerable<L> UnwrapLeft() =>
            collection.Where(e => e.IsLeft).Select(l => l.LeftValue);

        /// <summary>
        /// Unwraps the right values from a collection of Either objects.
        /// </summary>
        /// <returns>An IEnumerable of right values where the Either object is a Right.</returns>
        [Pure]
        public IEnumerable<R> UnwrapRight() =>
            collection.Where(e => e.IsRight).Select(r => r.RightValue);
    }
}
