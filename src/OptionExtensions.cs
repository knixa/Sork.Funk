﻿using System.Diagnostics.Contracts;

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

    /// <summary>
    /// Converts an <see cref="IEnumerable{T}"/> to an <see cref="Option{T}"/> containing a <see cref="NonEmptyList{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The input collection to convert.</param>
    /// <returns>
    /// An <see cref="Option{NonEmptyList{T}}"/> containing a <see cref="NonEmptyList{T}"/> if the collection has elements,
    /// or <see cref="Option{NonEmptyList{T}}.None"/> if the collection is null or empty.
    /// </returns>
    /// <remarks>
    /// This method ensures that if a <see cref="NonEmptyList{T}"/> is created, it will contain at least one element.
    /// </remarks>
    [Pure]
    public static Option<NonEmptyList<T>> ToNonEmptyOption<T>(this IEnumerable<T>? collection)
    {
        if (collection is null)
        {
            return Option<NonEmptyList<T>>.None;
        }

        var enumerable = collection.ToList();
        return enumerable.Count == 0
            ? Option<NonEmptyList<T>>.None
            : Option<NonEmptyList<T>>.Some(new NonEmptyList<T>(enumerable));
    }
}
