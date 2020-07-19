// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    using Reflection;
    using Linq;

    /// <summary>
    /// Contains extensions to <see cref="IDictionary"/>.
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
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Source object to convert.</param>
        /// <param name="keyCasing">The casing for the outputted key.</param>
        /// <param name="bindingAttr">Types of attributes to bind (if need to be specific).</param>
        /// <returns>IDictionary object representing the object passed in.</returns>
        /// <exception cref="InvalidCastException">Cannot automatically cast enumerable to dictionary</exception>
        public static IDictionary<string, object> AsDictionary<T>(this T source, StringCasing keyCasing = StringCasing.Unchanged, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
            where T : class, new()
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name.WithCasing(keyCasing),
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

        /// <summary>
        /// Builds a dictionary of all reflected properties of an object, using a delimiter to denoate sub-type properties
        /// i.e. a class could be reflected as:
        /// "Prop1"    "Value1"
        /// "Prop2:A"  "Value2"
        /// "Prop2:B"  "Value3"
        /// "Prop2:C"  "Value4"
        /// "Prop3"    true
        /// "Prop4"    500
        /// </summary>
        /// <typeparam name="T">Type of object to reflect.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keyDelimiter">The delimiter.</param>
        /// <param name="keyCasing">The string casing for outputted keys.</param>
        /// <param name="maskSensitiveData">If set to <c>true</c> mask pii data (properties marked with PersonalData attribute).</param>
        /// <param name="bindingAttr">The binding attribute.</param>
        /// <returns>Dictionary&lt;System.String, System.Object&gt;.</returns>
        public static Dictionary<string, object> AsFlatDictionary<T>(this T source, StringCasing keyCasing = StringCasing.Unchanged, bool maskSensitiveData = false, 
            string keyDelimiter = ":", BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
            where T : class, new()
        {
            // Return final resulting flat dictionary.
            return source.GetFlatDictionary(keyCasing, keyDelimiter, string.Empty, maskSensitiveData, bindingAttr);
        }

        /// <summary>
        /// Builds a dictionary of all reflected properties of an object, using a delimiter to denoate sub-type properties
        /// i.e. a class could be reflected as:
        /// "Prop1"    "Value1"
        /// "Prop2:A"  "Value2"
        /// "Prop2:B"  "Value3"
        /// "Prop2:C"  "Value4"
        /// "Prop3"    true
        /// "Prop4"    500
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="keyDelimiter">The delimiter.</param>
        /// <param name="maskSensitiveData">If set to <c>true</c> mask pii data (properties marked with PersonalData attribute).</param>
        /// <param name="keyCasing">The string casing for outputted keys.</param>
        /// <param name="bindingAttr">The binding attribute.</param>
        /// <returns>Dictionary&lt;System.String, System.Object&gt;.</returns>
        public static Dictionary<string, object> AsFlatDictionary(this object source, StringCasing keyCasing = StringCasing.Unchanged, 
            bool maskSensitiveData = false, string keyDelimiter = ":", BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            // Return final resulting flat dictionary.
            return source.GetFlatDictionary(keyCasing, keyDelimiter, string.Empty, maskSensitiveData, bindingAttr);
        }

        private static Dictionary<string, object> GetFlatDictionary<T>(this T source, StringCasing keyCasing = StringCasing.Unchanged, string keyDelimiter = ":", 
            string prefix = "", bool maskSensitiveData = false, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
            where T : class, new()
        {
            var returnDict = new Dictionary<string, object>();

            // Return empty dictionary if the source is null.
            if (source == null)
            {
                return returnDict;
            }

            var rootItems = source.GetType().GetProperties(bindingAttr);

            // If the reflected values length is zero, just add now to the dictionary.
            if (rootItems.Length == 0)
            {
                returnDict.Add(prefix, source);
            }
            else
            {
                if (!prefix.IsNullOrEmpty())
                    prefix += keyDelimiter;

                // Loop through each reflected property in order to build up the returned dictionary key/values.
                foreach (var item in rootItems)
                {
                    var value = item.GetValue(source, null);
                    var key = $"{prefix}{item.Name}".WithCasing(keyCasing);

                    if (value == null || value.Equals(item.PropertyType.GetDefault()))
                    {
                        // If we dont have a value, add as empty string.
                        returnDict.Add(key, null);
                    }
                    else if (item.PropertyType.IsEnumerableType())
                    {
                        // If this is an enumerable, then loop through each item and get its value for the dictionary.
                        IEnumerable vals = value as IEnumerable;
                        var index = 0;

                        foreach (var val in vals)
                        {
                            returnDict.AddRange(val.GetFlatDictionary(keyCasing, keyDelimiter, $"{key}[{index}]", maskSensitiveData, bindingAttr));
                            index++;
                        }
                    }
                    else if (item.PropertyType.IsSystemType())
                    {
                        object maskValue = value;

                        if (maskSensitiveData && item.IsSensitiveOrPersonalData())
                        {
                            if (item.PropertyType == typeof(string) && (value as string).Length > 0)
                            {
                                // Strings get asterix as mask instead.
                                maskValue = "*****";
                            }
                            else
                            {
                                // If we should mask pii data, then use default of type or null, rather than the real value.
                                maskValue = item.PropertyType.GetDefault();
                            }
                        }

                        // If this is a plain old system type, then just add straight into the dictionary.
                        returnDict.Add($"{key}", maskValue);
                    }
                    else
                    {
                        // Otherwise, reflect all properties of this complex type in the next level of the dictionary.
                        returnDict.AddRange(value.GetFlatDictionary(keyCasing, keyDelimiter, key, maskSensitiveData, bindingAttr));
                    }
                }
            }

            // Return final resulting flat dictionary.
            return returnDict;
        }

    }
}
