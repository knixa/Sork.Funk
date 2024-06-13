using System.Diagnostics.Contracts;

namespace Sork.Funk;

public static class OptionExtensions
{
    /// <summary>
    /// Unwraps the values from a collection of Option objects.
    /// </summary>
    /// <typeparam name="T">The type of the value. T must not be null.</typeparam>
    /// <param name="collection">The collection of Option objects.</param>
    /// <returns>An IEnumerable of values where the Option object is Some.</returns>
    [Pure]
    public static IEnumerable<T> Unwrap<T>(this IEnumerable<Option<T>> collection) where T : notnull =>
        collection.Where(val => val.IsSome)
            .Select(val => val.Value!);
}
