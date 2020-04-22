// ReSharper disable once CheckNamespace
namespace System
{
    /// <summary>
    /// Extension methods for Long.
    /// </summary>
    public static class LongExtensions
    {
        internal static readonly DateTime Utc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts from Epoch time (number of seconds since 1st Jan 1970)
        /// </summary>
        /// <param name="epochTime">Epoch time</param>
        /// <returns>DateTime representation of Epoch time</returns>
        public static DateTime ToDateTime(this long epochTime)
        {
            return Utc.ToLocalTime().AddTicks(epochTime);
        }

        /// <summary>
        /// Convert a DateTime to epoch.
        /// </summary>
        /// <param name="dt">DateTime to convert to an epoch time.</param>
        /// <returns>Double epoch representation of a datetime.</returns>
        public static long ToEpochTime(this DateTime dt)
        {
            var t = dt.ToUniversalTime() - Utc;
            return t.Ticks;
        }
    }
}
