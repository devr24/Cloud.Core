namespace Cloud.Core.Extensions
{
    using System.Diagnostics;
    using System.Text.Json;

    /// <summary>Json converter extensions.</summary>
    public static class JsonConvertExtensions
    {
        /// <summary>
        /// Tries the deserialize a string.
        /// </summary>
        /// <typeparam name="T">Generic type to deserialize to.</typeparam>
        /// <param name="source">The source json string.</param>
        /// <returns>T object to deserialize to.</returns>
        public static T TryDeserialize<T>(string source) where T : new()
        {
            try
            {
                // Attempt to convert and catch exception if caught.
                return JsonSerializer.Deserialize<T>(source);
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
                return default;
            }
        }
    }
}
