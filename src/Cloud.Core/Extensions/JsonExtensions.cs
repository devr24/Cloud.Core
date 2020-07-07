namespace Newtonsoft.Json
{
    using System.Diagnostics;

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
                return JsonConvert.DeserializeObject<T>(source);
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
                return default;
            }
        }
    }
}
