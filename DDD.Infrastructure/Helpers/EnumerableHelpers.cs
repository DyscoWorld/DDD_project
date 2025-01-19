namespace DDD.Infrastructure.Helpers;

public static class EnumerableHelpers
{
    private static Random Random { get; set; } = new();

    public static IEnumerable<T> RandomPermutation<T>(this IEnumerable<T> sequence)
    {
        T[] retArray = sequence.ToArray();

        for (int i = 0; i < retArray.Length - 1; i += 1)
        {
            int swapIndex = Random.Next(i, retArray.Length);
            if (swapIndex != i)
            {
                T temp = retArray[i];
                retArray[i] = retArray[swapIndex];
                retArray[swapIndex] = temp;
            }
        }

        return retArray;
    }
}
