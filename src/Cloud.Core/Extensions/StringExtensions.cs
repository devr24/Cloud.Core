// ReSharper disable once CheckNamespace
namespace System
{
    using System.IO;
    using System.Text;
    using Text.RegularExpressions;

    /// <summary>
    /// String Extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Gets the memory footprint (size) in bytes.
        /// </summary>
        /// <param name="str">The string to check length for.</param>
        /// <param name="encoding">The encoding of the string.</param>
        /// <returns>Long - length of string.</returns>
        public static long GetSizeInBytes(this string str, Encoding encoding)
        {
            if (str.IsNullOrEmpty())
                return 0;

            return encoding.GetByteCount(str);
        }

        /// <summary>
        /// Determines whether string is null or zero length.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>
        ///   <c>true</c> if [is null or empty] [the specified string]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Checks whether the string value is either <c>null</c> or white space.
        /// </summary>
        /// <param name="value"><see cref="string"/> value to check.</param>
        /// <returns>Returns <c>True</c>, if the string value is either <c>null</c> or white space; otherwise returns <c>False</c>.</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException" /> if the given value is <c>null</c> or white space.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>
        /// Returns the original value, if the value is NOT <c>null</c>; otherwise throws an <see cref="ArgumentNullException" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">value</exception>
        public static string ThrowIfNullOrWhiteSpace(this string value)
        {
            if (value.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(value));
            }

            return value;
        }

        /// <summary>
        /// Adds the space before caps.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        /// String with spaces before caps
        /// </returns>
        public static string AddSpaceBeforeCaps(this string str)
        {
            if (str == null)
                return null;

            return Regex.Replace(str, "([a-z])([A-Z])", "$1 $2");
        }

        /// <summary>
        /// Attempt to convert string to guid.
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <returns><see cref="Guid" /> representing the string or empty <see cref="Guid" />.</returns>
        public static Guid ToGuid(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return Guid.Empty;
            }

            if (Guid.TryParse(str, out Guid newGuid))
            {
                return newGuid;
            }

            return Guid.Empty;
        }

        /// <summary>
        /// Checks whether the string value is equal to the comparer, regardless of casing.
        /// </summary>
        /// <param name="value">Value to compare.</param>
        /// <param name="comparer">Comparing value.</param>
        /// <returns>
        /// Returns <c>True</c>, if the string value is equal to the comparer, regardless of casing; otherwise returns <c>False</c>.
        /// </returns>
        public static bool IsEquivalentTo(this string value, string comparer)
        {
            return value.Equals(comparer, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Converts the string value to <see cref="int"/> value.
        /// </summary>
        /// <param name="value">String value to convert.</param>
        /// <returns>Returns the <see cref="int"/> value converted.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/></exception>
        public static int ToInt32(this string value)
        {
            value.ThrowIfNullOrWhiteSpace();

            return Convert.ToInt32(value);
        }

        /// <summary>
        /// Converts the string value to <see cref="bool"/> value.
        /// </summary>
        /// <param name="value">String value to convert.</param>
        /// <returns>Returns the <see cref="bool"/> value converted.</returns>
        public static bool ToBoolean(this string value)
        {
            value.ThrowIfNullOrWhiteSpace();

            string normalizedString = (value?.Trim() ?? "false").ToLowerInvariant();
            bool result = (normalizedString.StartsWith("y")
                           || normalizedString.StartsWith("t")
                           || normalizedString.StartsWith("1"));
            return result;
        }

        /// <summary>
        /// Checks whether the given list of items contains the item or not, regardless of casing.
        /// </summary>
        /// <param name="value">Value to compare.</param>
        /// <param name="comparer">Comparing value.</param>
        /// <returns>Returns <c>True</c>, if the string value contains the comparer, regardless of casing; otherwise returns <c>False</c>.</returns>
        public static bool ContainsEquivalent(this string value, string comparer)
        {
            value.ThrowIfNullOrWhiteSpace();

            return value.ToLowerInvariant().Contains(comparer.ToLowerInvariant());
        }

        /// <summary>
        /// Checks whether the string value starts with the comparer, regardless of casing.
        /// </summary>
        /// <param name="value">Value to compare.</param>
        /// <param name="comparer">Comparing value.</param>
        /// <returns>
        /// Returns <c>True</c>, if the string value starts with the comparer, regardless of casing; otherwise returns <c>False</c>.
        /// </returns>
        public static bool StartsWithEquivalent(this string value, string comparer)
        {
            value.ThrowIfNullOrWhiteSpace();

            return value.StartsWith(comparer, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Checks whether the string value ends with the comparer, regardless of casing.
        /// </summary>
        /// <param name="value">Value to compare.</param>
        /// <param name="comparer">Comparing value.</param>
        /// <returns>Returns <c>True</c>, if the string value ends with the comparer, regardless of casing; otherwise returns <c>False</c>.</returns>
        public static bool EndsWithEquivalent(this string value, string comparer)
        {
            value.ThrowIfNullOrWhiteSpace();

            return value.EndsWith(comparer, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Converts string to stream.
        /// </summary>
        /// <param name="value">The string value to convert.</param>
        /// <param name="encoding">The encoding of the string.</param>
        /// <returns>Stream version of string.</returns>
        public static MemoryStream ConvertToStream(this string value, Encoding encoding)
        {
            value.ThrowIfNullOrWhiteSpace();
            return new MemoryStream(encoding.GetBytes(value));
        }

        /// <summary>
        /// Froms the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>System.String.</returns>
        public static string ConvertToString(this MemoryStream stream)
        {
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
