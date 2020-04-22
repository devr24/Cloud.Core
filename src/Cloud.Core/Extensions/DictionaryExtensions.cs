// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    using Reflection;
    using Linq;

    /// <summary>
    /// Contains extensions to <see cref="System.Collections.IDictionary"/>.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Allows an existing dictionary the capability of adding a range of existing keyvalue pairs.
        /// </summary>
        /// <typeparam name="T">Source dictionary to add to.</typeparam>
        /// <typeparam name="O">Range dictionary to take items from and append to source.</typeparam>
        /// <param name="source">The source to append to.</param>
        /// <param name="range">The range to take from.</param>
        /// <returns>Dictionary&lt;T, O&gt;.</returns>
        public static void AddRange<T, O>(this Dictionary<T, O> source, Dictionary<T, O> range)
        {
            foreach (var item in range)
            {
                source.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// Append a KeyValuePair array to the source dictionary.
        /// </summary>
        /// <typeparam name="T">Source dictionary to add to.</typeparam>
        /// <typeparam name="O">Range KeyValuePair array to take items from and append to source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="range">The range.</param>
        /// <returns>Dictionary&lt;T, O&gt;.</returns>
        public static void AddRange<T, O>(this Dictionary<T, O> source, KeyValuePair<T, O>[] range)
        {
            foreach (var item in range)
            {
                source.Add(item.Key, item.Value);
            }
        }

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

        /// <summary>Returns a <see cref="string" /> that represents this instance.</summary>
        /// <param name="collection">The collection to concatenate.</param>
        /// <param name="delimiter">Delimiter to join the values with.  Defaults to ";".</param>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public static string ToDelimitedString(this Dictionary<string, string> collection, string delimiter = ";")
        {
            return string.Join(delimiter, collection.Select(x => x.Key + "=" + x.Value).ToArray()); 
        }

        /// <summary>
        /// Converts a dictionary to object of type T.
        /// </summary>
        /// <typeparam name="T">Target type to convert to.</typeparam>
        /// <param name="source">Source dictionary to convert into generic object type.</param>
        /// <returns>Type T object.</returns>
        public static T ToObject<T>(this IDictionary<string, object> source) where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
            {
                someObjectType.GetProperty(item.Key)?.SetValue(someObject, item.Value, null);
            }

            return someObject;
        }

        /// <summary>
        /// Converts object to IDictionary{string,object}.
        /// </summary>
        /// <param name="source">Source object to convert.</param>
        /// <param name="bindingAttr">Types of attributes to bind (if need to be specific).</param>
        /// <returns>IDictionary object representing the object passed in.</returns>
        public static IDictionary<string, object> AsDictionary(this object source, 
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );
        }

        /// <summary>
        /// Convert a dictionary to a list of key value pairs.
        /// </summary>
        /// <typeparam name="T">Key type.</typeparam>
        /// <typeparam name="J">Value type.</typeparam>
        /// <param name="source">Source dictionary to convert.</param>
        /// <returns>List of key value pairs representing the dictionary.</returns>
        public static List<KeyValuePair<T, J>> ToList<T, J>(this Dictionary<T, J> source)
        {
            var listOfProps = new List<KeyValuePair<T, J>>();
            foreach (var prop in source)
            {
                listOfProps.Add(new KeyValuePair<T, J>(prop.Key, prop.Value));
            }
            return listOfProps;
        }

        /// <summary>
        /// Convert a dictionary to an array of key value pairs.
        /// </summary>
        /// <typeparam name="T">Key type.</typeparam>
        /// <typeparam name="J">Value type.</typeparam>
        /// <param name="source">Source dictionary to convert.</param>
        /// <returns>List of key value pairs representing the dictionary.</returns>
        public static KeyValuePair<T, J>[] ToArray<T, J>(this Dictionary<T, J> source)
        {
            // Convert the source using the ToList extension and then convert the results of that to an array.
            return source.ToList().ToArray();
        }

        /// <summary>
        /// Either add a new item, or if it already exists, update it.
        /// </summary>
        /// <typeparam name="T">Key type.</typeparam>
        /// <typeparam name="J">Value type.</typeparam>
        /// <param name="source">Source dictionary to add/update.</param>
        /// <param name="key">Key for the element.</param>
        /// <param name="value">Value for the element.</param>
        /// <returns></returns>
        public static Dictionary<T, J> AddOrUpdate<T, J>(this Dictionary<T, J> source, T key, J  value)
        {
            if (!source.TryAdd(key, value))
            {
                source[key] = value;
            }
            return source;
        }

        /// <summary>
        /// Either add a new item, or if it already exists, update it.
        /// </summary>
        /// <typeparam name="T">Key type.</typeparam>
        /// <typeparam name="J">Value type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>IDictionary&lt;T, J&gt;.</returns>
        public static IDictionary<T, J> AddOrUpdate<T, J>(this IDictionary<T, J> source, T key, J value)
        {
            if (!source.TryAdd(key, value))
            {
                source[key] = value;
            }
            return source;
        }

        /// <summary>
        /// Either add a new item, or if it already exists, update it.
        /// </summary>
        /// <typeparam name="T">Key type.</typeparam>
        /// <typeparam name="J">Value type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="prop">The property.</param>
        /// <returns>IDictionary&lt;T, J&gt;.</returns>
        public static IDictionary<T, J> AddOrUpdate<T, J>(this IDictionary<T, J> source, KeyValuePair<T,J> prop)
        {
            if (!source.TryAdd(prop.Key, prop.Value))
            {
                source[prop.Key] = prop.Value;
            }
            return source;
        }

        /// <summary>
        /// Either add a new item, or if it already exists, update it.
        /// </summary>
        /// <typeparam name="T">Key type.</typeparam>
        /// <typeparam name="J">Value type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="prop">The property.</param>
        /// <returns>Dictionary&lt;T, J&gt;.</returns>
        public static Dictionary<T, J> AddOrUpdate<T, J>(this Dictionary<T, J> source, KeyValuePair<T, J> prop)
        {
            if (!source.TryAdd(prop.Key, prop.Value))
            {
                source[prop.Key] = prop.Value;
            }
            return source;
        }
    }
}
