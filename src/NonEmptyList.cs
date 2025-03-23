namespace Sork.Funk;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a list that is guaranteed to contain at least one element.
/// </summary>
/// <typeparam name="T">The type of the elements in the list.</typeparam>
public class NonEmptyList<T> : IReadOnlyList<T>
{
    private readonly List<T> _list;

    internal NonEmptyList(List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            throw new ArgumentException("NonEmptyList must contain at least one element.");
        }

        _list = list;
    }

    /// <summary>
    /// Creates a <see cref="NonEmptyList{T}"/> from an IEnumerable of items.
    /// </summary>
    /// <param name="items">The IEnumerable of items to populate the list.</param>
    /// <returns>A new instance of <see cref="NonEmptyList{T}"/> containing the provided items.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="items"/> argument is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="items"/> argument is empty.</exception>
    public static NonEmptyList<T> Create(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        var list = items.ToList();
        if (list.Count == 0)
        {
            throw new ArgumentException("NonEmptyList must contain at least one element.");
        }

        return new NonEmptyList<T>(list);
    }

    /// <summary>
    /// Creates a <see cref="NonEmptyList{T}"/> from a head element and optional tail elements.
    /// </summary>
    /// <param name="head">The first element of the list.</param>
    /// <param name="tail">The optional additional elements of the list.</param>
    /// <returns>A new instance of <see cref="NonEmptyList{T}"/> containing the provided elements.</returns>
    public static NonEmptyList<T> Create(T head, params T[] tail)
    {
        var list = new List<T> { head };
        list.AddRange(tail);
        return new NonEmptyList<T>(list);
    }

    /// <summary>
    /// Gets the first element of the list.
    /// </summary>
    public T Head => _list.First();

    /// <summary>
    /// Gets all elements of the list except the first one.
    /// </summary>
    public IReadOnlyList<T> Tail => _list.Skip(1).ToList();

    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <returns>The element at the specified index.</returns>
    public T this[int index] => _list[index];

    /// <summary>
    /// Gets the number of elements in the list.
    /// </summary>
    public int Count => _list.Count;

    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
}
