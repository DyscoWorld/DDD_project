namespace DDD.Infrastructure;

public static class EnumerableExtensions
{
    public static T Pop<T>(this IList<T> source)
    {
        if (source == null || source.Count == 0)
            throw new InvalidOperationException("Невозможно извлечь элемент из пустой коллекции.");

        var result = source[^1];
        source.RemoveAt(source.Count - 1);
        return result;
    }
}