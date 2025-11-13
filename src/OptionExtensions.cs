using System.Diagnostics.Contracts;

namespace Sork.Funk;

public static class OptionExtensions
{
    extension<T>(IEnumerable<Option<T>> collection) where T : notnull
    {
        /// <summary>
        /// Unwraps the values from a collection of Option objects.
        /// </summary>
        /// <typeparam name="T">The type of the value. T must not be null.</typeparam>
        /// <returns>An IEnumerable of values where the Option is Some.</returns>
        [Pure]
        public IEnumerable<T> Unwrap() =>
            collection.Where(val => val.IsSome)
                .Select(val => val.Value!);
    }

    extension<T>(IEnumerable<T>? collection) where T : IEquatable<T>
    {
        /// <summary>
        /// Converts an <see cref="IEnumerable{T}"/> to an <see cref="Option{T}"/> containing a <see cref="NonEmptyList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <returns>
        /// An <see cref="Option{T}"/> containing a <see cref="NonEmptyList{T}"/> if the collection has elements,
        /// or <see cref="Option{T}.None"/> if the collection is null or empty.
        /// </returns>
        [Pure]
        public Option<NonEmptyList<T>> ToNonEmptyOption()
        {
            if (collection is null)
            {
                return Option<NonEmptyList<T>>.None;
            }

            var enumerable = collection.ToArray();
            return enumerable.Length == 0
                ? Option<NonEmptyList<T>>.None
                : Option<NonEmptyList<T>>.Some(new NonEmptyList<T>(enumerable));
        }
    }
}
