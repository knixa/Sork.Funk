namespace Sork.Funk;

public static class OptionExtensions
{
    public static IEnumerable<T> Unwrap<T>(this IEnumerable<Option<T>> collection) where T : notnull => 
        collection.Where(val => val.IsSome)
            .Select(val => val.Value!);
}