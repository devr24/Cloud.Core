// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    using Linq;
    using Concurrent;
    using Threading.Tasks;

    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Parallels for each asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="funcBody">The function body.</param>
        /// <param name="maxDoP">The maximum degrees of parallelism.</param>
        /// <returns>Task.</returns>
        public static Task ParallelForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> funcBody, int maxDoP = 0)
        {
            if (maxDoP == 0 || maxDoP > Environment.ProcessorCount)
            {
                maxDoP = Environment.ProcessorCount;
            }

            async Task AwaitPartition(IEnumerator<T> partition)
            {
                using (partition)
                {
                    while (partition.MoveNext())
                    {
                        await funcBody(partition.Current);
                    }
                }
            }

            return Task.WhenAll(
                Partitioner
                    .Create(source)
                    .GetPartitions(maxDoP)
                    .AsParallel()
                    .Select(AwaitPartition));
        }

        /// <summary>
        /// Checks whether the given list of items contains the item or not, regardless of casing.
        /// </summary>
        /// <param name="items">List of items.</param>
        /// <param name="item">Item to check.</param>
        /// <returns>Returns <c>True</c>, if the list of items contains the item; otherwise returns <c>False</c>.</returns>
        public static bool ContainsEquivalent(this IEnumerable<string> items, string item)
        {
            var itemsList = items as IList<string> ?? items.ToList();
            itemsList.ThrowIfNullOrDefault();

            return itemsList.Any(p => p.IsEquivalentTo(item));
        }

        // Taken from http://josheinstein.com/blog/2009/12/ienumerable-batch/
        /// <summary>
        /// Enumerates a sequence in chunks, yielding batches of a certain size to the enumerator.
        /// </summary>
        /// <typeparam name="T">The type of item in the batch.</typeparam>
        /// <param name="sequence">The sequence of items to be enumerated.</param>
        /// <param name="batchSize">The maximum number of items to include in a batch.</param>
        /// <returns>A sequence of arrays, with each array containing at most
        /// <paramref name="batchSize"/> elements.</returns>
        public static IEnumerable<T[]> Batch<T>(this IEnumerable<T> sequence, int batchSize)
        {
            var batch = new List<T>(batchSize);

            foreach (var item in sequence)
            {
                batch.Add(item);

                // When we've accumulated enough in the batch, send it out
                if (batch.Count >= batchSize)
                {
                    yield return batch.ToArray();
                    batch.Clear();
                }
            }

            // Send out any leftovers
            if (batch.Count > 0)
            {
                yield return batch.ToArray();
                batch.Clear();
            }
        }
    }
}
