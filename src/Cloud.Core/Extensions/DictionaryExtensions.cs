// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    using System;
    using Linq;

    /// <summary>
    /// Contains extensions to <see cref="System.Collections.IDictionary"/>.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Releases a <see cref="IDictionary{TKey,TValue}"/> where <typeparamref name="TValue"/> implements <see cref="IDisposable"/> by calling
        /// Dispose() on all it's values and then clearing it.
        /// </summary>
        /// <param name="source">The source <see cref="IDictionary{TKey,TValue}"/> that we want to release.</param>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        public static void Release<TKey, TValue>(this IDictionary<TKey, TValue> source) where TValue : IDisposable
        {
            foreach (var key in source.Keys)
            {
                source[key].Dispose();
            }

            source.Clear();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <param name="collection">The collection to concatenate.</param>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public static string ToDelimitedString(this Dictionary<string, string> collection)
        {
            return string.Join(";", collection.Select(x => x.Key + "=" + x.Value).ToArray()); 
        }
    }
}
