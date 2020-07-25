namespace Newtonsoft.Json.Linq
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>Json Conversion Extensions.</summary>
    public static class JsonConversionExtensions
    {
        /// <summary>Converts JObject to array.</summary>
        /// <param name="array">The array.</param>
        /// <returns>System.Object[].</returns>
        public static object[] ToArray(this JArray array)
        {
            return array.ToObject<object[]>().Select(ProcessArrayEntry).ToArray();
        }

        /// <summary>Converts JObject to dictionary.</summary>
        /// <param name="json">The json.</param>
        /// <returns>Dictionary&lt;System.String, System.Object&gt;.</returns>
        public static Dictionary<string, object> ToDictionary(this JObject json)
        {
            var propertyValuePairs = json.ToObject<Dictionary<string, object>>();
            ProcessJObjectProperties(propertyValuePairs);
            ProcessJArrayProperties(propertyValuePairs);
            return propertyValuePairs;
        }

        private static void ProcessJObjectProperties(Dictionary<string, object> propertyValuePairs)
        {
            var objectPropertyNames = (from property in propertyValuePairs
                let propertyName = property.Key
                let value = property.Value
                where value is JObject
                select propertyName).ToList();

            objectPropertyNames.ForEach(propertyName => propertyValuePairs[propertyName] = ToDictionary((JObject)propertyValuePairs[propertyName]));
        }

        private static void ProcessJArrayProperties(Dictionary<string, object> propertyValuePairs)
        {
            var arrayPropertyNames = (from property in propertyValuePairs
                let propertyName = property.Key
                let value = property.Value
                where value is JArray
                select propertyName).ToList();

            arrayPropertyNames.ForEach(propertyName => propertyValuePairs[propertyName] = ToArray((JArray)propertyValuePairs[propertyName]));
        }

        private static object ProcessArrayEntry(object value)
        {
            if (value is JObject)
            {
                return ToDictionary((JObject)value);
            }
            if (value is JArray)
            {
                return ToArray((JArray)value);
            }
            return value;
        }
    }
}
