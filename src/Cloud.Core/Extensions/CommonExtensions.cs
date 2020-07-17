namespace Cloud.Core.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>List of common extensions that are not placed in the default namespaces.</summary>
    public static class CommonExtensions
    {
        /// <summary>
        /// Builds a dictionary of all reflected properties of an object, using a delimiter to denoate sub-type properties
        /// i.e. a class could be reflected as:
        /// "Prop1"    "Value1"
        /// "Prop2:A"  "Value2"
        /// "Prop2:B"  "Value3"
        /// "Prop2:C"  "Value4"
        /// "Prop3"    "true"
        /// "Prop4"    "500"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keyDelimiter">The delimiter.</param>
        /// <param name="keyCasing">The string casing for outputted keys.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="bindingAttr">The binding attribute.</param>
        /// <returns>Dictionary&lt;System.String, System.Object&gt;.</returns>
        public static Dictionary<string, string> AsFlatStringDictionary<T>(this T source, StringCasing keyCasing = StringCasing.Unchanged, string keyDelimiter = ":", string prefix = "", BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                where T : class, new()
        {
            var returnDict = new Dictionary<string, string>();
            var rootItems = source.GetType().GetProperties(bindingAttr);

            // If the reflected values length is zero, just add now to the dictionary.
            if (rootItems.Length == 0)
            {
                returnDict.Add(prefix, source == null ? string.Empty : source.ToString());
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

                    if (value == null)
                    {
                        // If we dont have a value, add as empty string.
                        returnDict.Add(key, "");
                    }
                    else if (item.PropertyType.IsEnumerableType())
                    {
                        // If this is an enumerable, then loop through each item and get its value for the dictionary.
                        IEnumerable vals = value as IEnumerable;
                        var index = 0;

                        foreach (var val in vals)
                        {
                            returnDict.AddRange(val.AsFlatStringDictionary(keyCasing, keyDelimiter, $"{key}[{index}]"));
                            index++;
                        }
                    }
                    else if (item.PropertyType.IsSystemType())
                    {
                        // If this is a plain old system type, then just add straight into the dictionary.
                        returnDict.Add($"{key}", value.ToString());
                    }
                    else
                    {
                        // Otherwise, reflect all properties of this complex type in the next level of the dictionary.
                        returnDict.AddRange(value.AsFlatStringDictionary(keyCasing, keyDelimiter, key));
                    }
                }
            }

            // Return final resulting flat dictionary.
            return returnDict;
        }
    }
}
