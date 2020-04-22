// ReSharper disable once CheckNamespace
namespace System.IO
{
    /// <summary>
    /// Extension methods for Stream.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads the contents of a stream into a memory stream and converts to a byte array.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] CopyToBytes(this Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
