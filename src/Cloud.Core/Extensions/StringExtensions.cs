// ReSharper disable once CheckNamespace
namespace System
{
    using IO;
    using Text;
    using Text.RegularExpressions;
    using Diagnostics.CodeAnalysis;
    
    /// <summary>
    /// String Extension methods.
    /// </summary>
    public static class StringExtensions
    {
        private static readonly Regex TextContentCleanExpression = new Regex("[\\W]{1,}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Cleans the content of unnecessary characters using the regular expression "[\\W]{1,}".  Replaces with a space.
        /// </summary>
        /// <param name="text">The text content to clean.</param>
        /// <returns>Cleaned System.String.</returns>
        public static string RemoveNonAlphanumericCharacters(this string text)
        {
            return TextContentCleanExpression.Replace(text, " ");
        }

        /// <summary>Defaults the string if null or emtpy.</summary>
        /// <param name="source">The source.</param>
        /// <param name="defaultValue">The default value.</param>
        public static string DefaultIfNullOrEmtpy(this string source, string defaultValue)
        {
            if (source.IsNullOrEmpty())
            {
                return defaultValue;
            }
            return source;
        } 

        /// <summary>
        /// Removes multiple strings from the source string.
        /// </summary>
        /// <param name="str">The string to modify.</param>
        /// <param name="find">The string sequences to find and remove.</param>
        /// <returns>System.String.</returns>
        public static string RemoveMultiple(this string str, params string[] find)
        {
            var sb = new StringBuilder(str);

            foreach (var s in find)
            {
                sb.Replace(s, string.Empty);
            }
        
            return sb.ToString().ToLower();
        }

        /// <summary>
        /// Replaces multiple strings with a replacement string.
        /// </summary>
        /// <param name="str">The string to modify.</param>
        /// <param name="replaceWith">The replace with string.</param>
        /// <param name="find">The string sequences to find and replace.</param>
        /// <returns>System.String.</returns>
        public static string ReplaceMultiple(this string str, string replaceWith, params string[] find)
        {
            var sb = new StringBuilder(str);

            foreach (var s in find)
            {
                sb.Replace(s, replaceWith);
            }

            return sb.ToString().ToLower();
        }

        /// <summary>
        /// Multiline string extension.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>System.String.</returns>
        public static string MultiLine(params string[] args)
        {
            return string.Join(Environment.NewLine, args);
        }

        /// <summary>
        /// Sets the default if null.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <param name="default">The default value if null.</param>
        /// <returns>System.String returned.</returns>
        public static string SetDefaultIfNullOrEmpty(this string str, string @default)
        {
            if (str.IsNullOrEmpty())
            {
                str = @default;
            }

            return str;
        }

        /// <summary>
        /// Replaces the specified separators.
        /// </summary>
        /// <param name="str">The string to perform replace on.</param>
        /// <param name="replaceChars">The chars to replace.</param>
        /// <param name="newVal">The new value.</param>
        /// <returns>string with characters replaced.</returns>
        public static string ReplaceAll(this string str, char[] replaceChars, string newVal)
        {
            var temp = str.Split(replaceChars, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(newVal, temp);
        }
        
        /// <summary>
        /// Gets the memory footprint (size) in bytes.
        /// </summary>
        /// <param name="str">The string to check length for.</param>
        /// <param name="encoding">The encoding of the string.</param>
        /// <returns>Long - length of string.</returns>
        public static long GetSizeInBytes(this string str, Encoding encoding)
        {
            if (str.IsNullOrEmpty())
            {
                return 0;
            }

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
        /// Throws an <see cref="ArgumentNullException" /> if the given value is <c>null</c>.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>
        /// Returns the original value, if the value is NOT <c>null</c>; otherwise throws an <see cref="ArgumentNullException" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">value</exception>
        public static string ThrowIfNull(this string value)
        {
            if (value == null)
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
            {
                return null;
            }

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
            value.ThrowIfNull();

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
            value.ThrowIfNull();

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
            value.ThrowIfNull();

            return value.EndsWith(comparer, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Converts string to stream.
        /// </summary>
        /// <param name="value">The string value to convert.</param>
        /// <param name="encoding">The encoding of the string.</param>
        /// <returns>Stream version of string.</returns>
        public static MemoryStream ConvertToStream(this string value, [NotNull]Encoding encoding)
        {
            value.ThrowIfNull();
            return new MemoryStream(encoding.GetBytes(value));
        }

        /// <summary>
        /// Converts string to byte array.
        /// </summary>
        /// <param name="value">The string value to convert.</param>
        /// <param name="encoding">The encoding of the string.</param>
        /// <returns>Byte array version of string.</returns>
        public static byte[] ConvertToBytes(this string value, [NotNull]Encoding encoding)
        {
            value.ThrowIfNull();
            return encoding.GetBytes(value);
        }

        /// <summary>
        /// Reads the contents of a stream and returns the string.
        /// </summary>
        /// <param name="stream">The stream to convert from.</param>
        /// <returns>System.String.</returns>
        public static string ReadContents(this MemoryStream stream)
        {
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Substrings the specified text using a start and finish search string.
        /// </summary>
        /// <param name="str">The string to substitute.</param>
        /// <param name="start">The start text to find.</param>
        /// <param name="end">The end text to find.</param>
        /// <returns>System.String.</returns>
        public static string Substring(this string str, string start, string end)
        {
            try
            {
                var indexOfStartStartSize = str.Remove(0, str.IndexOf(start, StringComparison.Ordinal) + start.Length);
                return indexOfStartStartSize.Remove(indexOfStartStartSize.IndexOf(end, StringComparison.Ordinal), indexOfStartStartSize.Length - indexOfStartStartSize.IndexOf(end, StringComparison.Ordinal)).Trim();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
