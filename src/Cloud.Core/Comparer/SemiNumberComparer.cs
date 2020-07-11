#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute
#pragma warning disable CS1658 // Warning is overriding an error
namespace Cloud.Core.Comparer
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Semi Numeric Comparer can be used to compare any two strings with semi-numeric properties.
    /// Implements the <see cref="IComparer{string}" />
    /// </summary>
    /// <seealso cref="IComparer{string}" />
    public class SemiNumericComparer : IComparer<string>
    {
        /// <summary>
        /// Compares two semi-numberic strings to see the order of the strings. Example "10B" compared to "10A" would return the order "10A", "10B".
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>System.Int32.</returns>
        public int Compare(string s1, string s2)
        {
            int s1r, s2r;
            var s1n = IsNumeric(s1, out s1r);
            var s2n = IsNumeric(s2, out s2r);

            if (s1n && s2n) return s1r - s2r;
            else if (s1n) return -1;
            else if (s2n) return 1;

            var num1 = Regex.Match(s1, @"\d+$");
            var num2 = Regex.Match(s2, @"\d+$");

            var onlyString1 = s1.Remove(num1.Index, num1.Length);
            var onlyString2 = s2.Remove(num2.Index, num2.Length);

            if (onlyString1 == onlyString2)
            {
                if (num1.Success && num2.Success) return Convert.ToInt32(num1.Value) - Convert.ToInt32(num2.Value);
                else if (num1.Success) return 1;
                else if (num2.Success) return -1;
            }

            return string.Compare(s1, s2, true);
        }

        /// <summary>
        /// Determines whether the specified value is numeric.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="result">The result of the check.</param>
        /// <returns><c>true</c> if the specified value is numeric; otherwise, <c>false</c>.</returns>
        public bool IsNumeric(string value, out int result)
        {
            return int.TryParse(value, out result);
        }
    }
}
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute
#pragma warning restore CS1658 // Warning is overriding an error
