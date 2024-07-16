using System.Diagnostics.Contracts;

namespace Sork.Funk;

public static class NullableExtensions
{
    /// <summary>
    /// Converts a nullable value to an <see cref="Option{T}"/>.
    /// </summary>
    /// <typeparam name="T">The underlying value type.</typeparam>
    /// <param name="val">The nullable value.</param>
    /// <returns>An <see cref="Option{T}"/> containing the value if it's not null; otherwise, <see cref="Option{T}.None"/>.</returns>
    [Pure]
    public static Option<T> ToOption<T>(this T? val) where T : struct =>
        val.HasValue
            ? Option<T>.Some(val.Value)
            : Option<T>.None;
}
