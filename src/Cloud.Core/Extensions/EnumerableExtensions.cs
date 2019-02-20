// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    using Linq;

    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
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
    }
}
