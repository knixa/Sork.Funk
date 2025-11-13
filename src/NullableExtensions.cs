using System.Diagnostics.Contracts;

namespace Sork.Funk;

public static class NullableExtensions
{
    extension<T>(T? val) where T : struct
    {
        /// <summary>
        /// Converts a nullable value to an <see cref="Option{T}"/>.
        /// </summary>
        /// <typeparam name="T">The underlying value type.</typeparam>
        /// <returns>An <see cref="Option{T}"/> containing the value if it's not null; otherwise, <see cref="Option{T}.None"/>.</returns>
        [Pure]
        public Option<T> ToOption() =>
            val.HasValue
                ? Option<T>.Some(val.Value)
                : Option<T>.None;
    }

    extension<T>(T? val) where T : class
    {
        /// <summary>
        /// Converts a nullable reference type to an <see cref="Option{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the reference type.</typeparam>
        /// <returns>
        /// An <see cref="Option{T}"/> representing the value if it is not null, 
        /// or <see cref="Option{T}.None"/> if the value is null.
        /// </returns>
        public Option<T> ToOption() => val is null ? Option<T>.None : Option<T>.Some(val);
    }
}
