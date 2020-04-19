namespace Newtonsoft.Json.Converters
{
    using System;
    using Newtonsoft.Json;

    public class GenericEnumStringConverter : StringEnumConverter
    {
        /// <summary>
        /// Override of the String Enum Converter to return the default instance of an Enum if null, or an empty string is passed through during Deserialization
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // If the value to convert is blank or null, and the type to convert to is an enum, return the default instance of the enum.
            if (string.IsNullOrWhiteSpace(reader.Value.ToString()) && objectType.IsEnum)
            {
                return Activator.CreateInstance(objectType);
            }

            try
            {
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
            catch (Exception)
            {
                return Activator.CreateInstance(objectType);
            }
        }
    }
}
