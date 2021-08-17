namespace Cloud.Core.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.ComponentModel;
    using System;

    /// <summary>
    /// Contains methods that extend the enum struct with utility and mapping methods.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the description attribute value associated with an enum value, or if not present
        /// returns the enum value ToString() value.
        /// </summary>
        /// <param name="value">The enum value we want to get the description from.</param>
        /// <returns>The value of the description attribute present in this enum value.</returns>
        public static string ToDescription(this Enum value)
        {
            var attributes =
                (DescriptionAttribute[])value.GetType()
                                              .GetField(value.ToString())
                                              .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0
                ? attributes[0].Description
                : value.ToString();
        }

        /// <summary>
        /// Converts enum of type T to list of KeyValuePair (string key, T value).
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="addSpacesToCapitals">bool flag for space</param>
        /// <returns>
        /// List of key value pairs (string, int) representing the passed in enum.
        /// </returns>
        public static List<string> ListFromEnum<TSource>(bool addSpacesToCapitals = false)
            where TSource : struct
        {
            // List that is populated to be returned.
            var list = new List<string>();

            var type = typeof(TSource);

            // Loop through each enum item and add to the return list.
            foreach (var e in Enum.GetValues(type))
            {
                list.Add(addSpacesToCapitals ? e.ToString().AddSpaceBeforeCaps() : e.ToString());
            }

            // Order by the keys and return.
            return list.OrderBy(s => s).ToList();
        }

        /// <summary>
        /// Convert Int to Enum
        /// </summary>
        /// <typeparam name="T">The generic object to convert to.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>Type T converted object.</returns>
        public static T ConvertIntToEnum<T>(this int value) where T : struct, IConvertible
        {
            return ConvertToEnum<T>(value);
        }

        /// <summary>
        /// Convert String to Enum
        /// </summary>
        /// <typeparam name="T">The generic object to convert to.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>Type T converted object.</returns>
        public static T ConvertStringToEnum<T>(this string value) where T : struct, IConvertible
        {
            return ConvertToEnum<T>(value);
        }

        /// <summary>
        /// Convert to Enum
        /// </summary>
        /// <typeparam name="T">The generic object to convert to.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>Enum of type T.</returns>
        /// <exception cref="ArgumentException">T must be an enumeration type</exception>
        public static T ConvertToEnum<T>(object value) where T : struct, IConvertible
        {
            var result = default(T);

            if (value is string s)
            {
                if (string.IsNullOrEmpty(s))
                {
                    return result;
                }

                return Enum.TryParse<T>(s, true, out var resOut) ? resOut : result;
            }

            if (value != null && int.TryParse(value.ToString(), out var tempType) && Enum.IsDefined(typeof(T), tempType))
            {
                result = (T)Enum.ToObject(typeof(T), tempType);
            }
            return result;
        }
    }
}
