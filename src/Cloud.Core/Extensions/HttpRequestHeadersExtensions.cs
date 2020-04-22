namespace System.Net.Http.Headers
{
    /// <summary>
    /// Extension methods for the HttpRequestHeaders.
    /// </summary>
    public static class HttpRequestHeadersExtensions
    {
        /// <summary>
        /// Find a particular header from a request and return its value (as a semi-colon, delimited string).
        /// </summary>
        /// <param name="headers">Request headers to parse.</param>
        /// <param name="headerName">Name of header to find.</param>
        /// <param name="delimiter">The delimiter. Defaults to ";".</param>
        /// <returns>String version of the values collection, delimited by semi-colon.</returns>
        public static string GetHeaderValue(this HttpRequestHeaders headers, string headerName, string delimiter = ";")
        {
            // Loop through the headers until the header with the key matching the headerName passed, then return its value.
            foreach (var header in headers)
            {
                if (header.Key == headerName)
                {
                    return string.Join(delimiter, header.Value);
                }
            }
            return null;
        }
    }
}
