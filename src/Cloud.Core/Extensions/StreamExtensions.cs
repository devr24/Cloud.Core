namespace Cloud.Core.Extensions
{
    using System.Threading.Tasks;
    using System;
    using System.IO;

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
        public static async Task<byte[]> ToBytes(this Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                if (input.CanSeek)
                {
                    input.Position = 0;
                }

                await input.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Converts the file stream to base64.
        /// </summary>
        /// <param name="stream">The stream to convert.</param>
        /// <returns>s.</returns>
        public static async Task<string> ToBase64(this Stream stream)
        {
            return Convert.ToBase64String(await stream.ToBytes());
        }
    }
}
