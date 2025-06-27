namespace EnumGenerator.Internals.Extensions;

internal static class EnumerableExtensions
{
	public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
	{
		HashSet<TKey> keys = [];
		foreach (TSource element in source)
		{
			if (keys.Add(keySelector(element)))
				yield return element;
		}
	}
}
