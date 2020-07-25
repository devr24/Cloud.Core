using System.Linq;
using Newtonsoft.Json.Linq;

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
        /// <param name="keyCasing">The string casing for outputted keys.</param>
        /// <param name="keyDelimiter">The delimiter.</param>
        /// <param name="maskPiiData">If set to <c>true</c> mask pii data (properties marked with PersonalData attribute).</param>
        /// <param name="bindingAttr">The binding attribute.</param>
        /// <returns>Dictionary&lt;System.String, System.Object&gt;.</returns>
        public static Dictionary<string, string> AsFlatStringDictionary<T>(this T source, StringCasing keyCasing = StringCasing.Unchanged, bool maskPiiData = false, 
            string keyDelimiter = ":", BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                where T : class, new()
        {
            // Return final resulting flat dictionary.
            return source.GetFlatDictionary(keyCasing, keyDelimiter, string.Empty, maskPiiData, bindingAttr);
        }

        /// <summary>
        /// Builds a dictionary of all reflected properties of a JObject, using a delimiter to denoate sub-type properties
        /// i.e. a class could be reflected as:
        /// "Prop1"    "Value1"
        /// "Prop2:A"  "Value2"
        /// "Prop2:B"  "Value3"
        /// "Prop2:C"  "Value4"
        /// "Prop3"    "true"
        /// "Prop4"    "500"
        /// </summary>
        /// <param name="source">The JToken source.</param>
        /// <param name="keyCasing">The string casing for outputted keys.</param>
        /// <param name="keyDelimiter">The delimiter.</param>
        /// <param name="maskPiiData">If set to <c>true</c> mask pii data (properties marked with PersonalData attribute).</param>
        /// <param name="bindingAttr">The binding attribute.</param>
        /// <returns>Dictionary&lt;System.String, System.Object&gt;.</returns>
        public static Dictionary<string, string> AsFlatStringDictionary(this JToken source, StringCasing keyCasing = StringCasing.Unchanged, bool maskPiiData = false,
            string keyDelimiter = ":", BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            // If the source is not set, return fault.
            if (source == null)
                return new Dictionary<string, string>();

            JObject inner = source.Root.Value<JObject>();
            var tokenDict = inner.ToDictionary();
            return tokenDict.GetFlatDictionary(keyCasing, keyDelimiter, string.Empty, maskPiiData, bindingAttr);
        }

        private static Dictionary<string, string> GetFlatDictionary<T>(this T source, StringCasing keyCasing = StringCasing.Unchanged, string keyDelimiter = ":", 
            string prefix = "", bool maskPiiData = false, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                where T : class, new()
        {
            var returnDict = new Dictionary<string, string>();

            // Return empty dictionary if the source is null.
            if (source == null)
                return returnDict;

            // If this is a dictionary, parse each element one by one.
            if (source.IsDictionary())
            {
                foreach (DictionaryEntry item in source as IDictionary)
                {
                    returnDict.AddRange(GetProperty(item.Key.ToString(), item.Value, prefix, keyCasing, keyDelimiter, maskPiiData, bindingAttr));
                }
            }
            else
            {
                // Otherwise, if this is an object, parse each property.
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
                        returnDict.AddRange(GetProperty(item.Name, item.GetValue(source, null), prefix, keyCasing, keyDelimiter, maskPiiData, bindingAttr));
                    }
                }
            }

            // Return final resulting flat dictionary.
            return returnDict;
        }

        private static Dictionary<string, string> GetProperty(string name, object value, string prefix, StringCasing keyCasing, string keyDelimiter, bool maskPiiData, BindingFlags bindingAttr)
        {
            var returnDict = new Dictionary<string, string>();
            var key = $"{prefix}{name}".WithCasing(keyCasing);
            Type valueType = value != null ? value.GetType() : null;

            if (value == null || value.Equals(valueType.GetDefault()))
            {
                // If we dont have a value, add as empty string.
                returnDict.Add(key, "");
            }
            else if (valueType.IsEnumerableType())
            {
                // If this is an enumerable, then loop through each item and get its value for the dictionary.
                IEnumerable vals = value as IEnumerable;
                var index = 0;

                foreach (var val in vals)
                {
                    returnDict.AddRange(val.GetFlatDictionary(keyCasing, keyDelimiter, $"{key}[{index}]", maskPiiData, bindingAttr));
                    index++;
                }
            }
            else if (valueType.IsSystemType())
            {
                // If this is a plain old system type, then just add straight into the dictionary.
                returnDict.Add($"{key}", maskPiiData && (valueType.GetPiiDataProperties().Any() || valueType.GetSensitiveInfoProperties().Any()) ? "*****" : value.ToString());
            }
            else
            {
                // Otherwise, reflect all properties of this complex type in the next level of the dictionary.
                returnDict.AddRange(value.GetFlatDictionary(keyCasing, keyDelimiter, key, maskPiiData, bindingAttr));
            }

            return returnDict;
        }
    }
}
