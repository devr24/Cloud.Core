using System;

namespace Cloud.Core.Extensions
{
    /// <summary>
    /// Extension methods for Integers.
    /// </summary>
    public static class IntExtensions
    {
        internal static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB" }; // extend if you REALLY want to...

        /// <summary>
        /// Sizes the suffix.
        /// </summary>
        /// <param name="value">The value to convert (byte length).</param>
        /// <param name="decimalPlaces">The decimal places.</param>
        /// <returns>String representation of bytes with suffix.</returns>
        /// <exception cref="ArgumentOutOfRangeException">decimalPlaces</exception>
        public static string ToSizeSuffix(this long value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException(nameof(decimalPlaces)); }
            if (value < 0) { return "-" + ToSizeSuffix(-value); }
            if (value == 0) { return  0.ToString("N" + decimalPlaces) + " bytes"; }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return adjustedSize.ToString("N" + decimalPlaces) + " " + SizeSuffixes[mag];
        }
    }
}
