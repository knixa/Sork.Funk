namespace Sork.Funk;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a list that is guaranteed to contain at least one element.
/// </summary>
/// <typeparam name="T">The type of the elements in the list.</typeparam>
public sealed class NonEmptyList<T> : IEquatable<NonEmptyList<T>>, IReadOnlyList<T> where T : IEquatable<T>
{
    private readonly T[] _list;

    internal NonEmptyList(T[] list) => _list = list;

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
        var list = items.ToArray();
        if (list.Length == 0)
        {
            throw new ArgumentException($"{nameof(items)} must contain at least one element.");
        }

        return new NonEmptyList<T>(list);
    }

    /// <summary>
    /// Creates a <see cref="NonEmptyList{T}"/> from a head element and optional tail elements.
    /// </summary>
    /// <param name="head">The first element of the list.</param>
    /// <param name="tail">The optional additional elements of the list.</param>
    /// <returns>A new instance of <see cref="NonEmptyList{T}"/> containing the provided elements.</returns>
    public static NonEmptyList<T> Create(T head, params T[] tail) => new([head, ..tail]);

    /// <summary>
    /// Gets the first element of the list.
    /// </summary>
    public T Head => _list.First();

    /// <summary>
    /// Gets all elements of the list except the first one.
    /// </summary>
    public IReadOnlyList<T> Tail => _list.Length == 1 ? [] : _list[1..];

    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <returns>The element at the specified index.</returns>
    public T this[int index] => _list[index];

    /// <summary>
    /// Gets the number of elements in the list.
    /// </summary>
    public int Count => _list.Length;

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_list).GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>)_list).GetEnumerator();

    public static bool operator !=(NonEmptyList<T> left, NonEmptyList<T>? right)
    {
        return !left.Equals(right);
    }

    public static bool operator ==(NonEmptyList<T> left, NonEmptyList<T>? right)
    {
        return left.Equals(right);
    }

    /// <inheritdoc/>
    public bool Equals(NonEmptyList<T>? other)
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals(this, other) || _list.AsSpan().SequenceEqual(other._list);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj.GetType() == GetType() && Equals((NonEmptyList<T>)obj);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        HashCode hashCode = default;

        foreach (var item in _list)
        {
            hashCode.Add(item);
        }
        return hashCode.ToHashCode();
    }
}
