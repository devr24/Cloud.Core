namespace Cloud.Core.Web
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for API Service
    /// </summary>
    public interface IApiService
    {
        /// <summary>
        /// Performs a GET request.
        /// </summary>
        /// <typeparam name="T">The object returned from the response.</typeparam>
        /// <param name="url">Url of GET request.</param>
        /// <param name="unsuccsessfulAction">Custom action to perform if request is not successful.</param>
        /// <returns>Async take with type T object.</returns>
        Task<T> Get<T>(string url, Action<HttpResponseMessage> unsuccsessfulAction = null);

        /// <summary>
        /// Performs a GET request.
        /// </summary>
        /// <typeparam name="T">The object returned from the response.</typeparam>
        /// <param name="url">Url of GET request.</param>
        /// <param name="headers">List of headers to set on the request.</param>
        /// <param name="unsuccsessfulAction">Custom action to perform if request is not successful.</param>
        /// <returns>Async take with type T object.</returns>
        Task<T> Get<T>(string url, Dictionary<string, string> headers, Action<HttpResponseMessage> unsuccsessfulAction = null);

        /// <summary>
        /// Performs a POST request.
        /// </summary>
        /// <typeparam name="T">The object returned from the response.</typeparam>
        /// <param name="url">Url of POST request.</param>
        /// <param name="content">Content of the POST body.</param>
        /// <param name="unsuccsessfulAction">Custom action to perform if request is not successful.</param>
        /// <returns>Async take with type T object.</returns>
        Task<T> Post<T>(string url,  HttpContent content, Action<HttpResponseMessage> unsuccsessfulAction = null);

        /// <summary>
        /// Performs a POST request.
        /// </summary>
        /// <typeparam name="T">The object returned from the response.</typeparam>
        /// <param name="url">Url of POST request.</param>
        /// <param name="headers">List of headers to set on the request.</param>
        /// <param name="content">Content of the POST body.</param>
        /// <param name="unsuccsessfulAction">Custom action to perform if request is not successful.</param>
        /// <returns>Async take with type T object.</returns>
        Task<T> Post<T>(string url, Dictionary<string, string> headers, HttpContent content = null, Action<HttpResponseMessage> unsuccsessfulAction = null);

        /// <summary>
        /// Performs a PUT request.
        /// </summary>
        /// <typeparam name="T">The object returned from the response.</typeparam>
        /// <param name="url">Url of PUT request</param>
        /// <param name="content">Content of the PUT body</param>
        /// <param name="unsuccsessfulAction">Custom action to perform if request is not successful</param>
        /// <returns>Async take with type T object.</returns>
        Task<T> Put<T>(string url, HttpContent content = null, Action<HttpResponseMessage> unsuccsessfulAction = null);

        /// <summary>
        /// Performs a PUT request.
        /// </summary>
        /// <typeparam name="T">The object returned from the response.</typeparam>
        /// <param name="url">Url of PUT request</param>
        /// <param name="headers">List of headers to set on the request.</param>
        /// <param name="content">Content of the PUT body</param>
        /// <param name="unsuccsessfulAction">Custom action to perform if request is not successful</param>
        /// <returns>Async take with type T object.</returns>
        Task<T> Put<T>(string url, Dictionary<string, string> headers, HttpContent content = null, Action<HttpResponseMessage> unsuccsessfulAction = null);

        /// <summary>
        /// Performs a DELETE request.
        /// </summary>
        /// <typeparam name="T">The object returned from the response.</typeparam>
        /// <param name="url">Url of DELETE request.</param>
        /// <param name="unsuccsessfulAction">Custom action to perform if request is not successful.</param>
        /// <returns>Async take with type T object.</returns>
        Task<T> Delete<T>(string url, Action<HttpResponseMessage> unsuccsessfulAction = null);

        /// <summary>
        /// Performs a DELETE request.
        /// </summary>
        /// <typeparam name="T">The object returned from the response.</typeparam>
        /// <param name="url">Url of DELETE request.</param>
        /// <param name="headers">List of headers to set on the request.</param>
        /// <param name="unsuccsessfulAction">Custom action to perform if request is not successful.</param>
        /// <returns>Async take with type T object.</returns>
        Task<T> Delete<T>(string url, Dictionary<string, string> headers = null, Action<HttpResponseMessage> unsuccsessfulAction = null);

        /// <summary>
        /// Executes the request and returns the typed response body.
        /// </summary>
        /// <typeparam name="T">The object returned from the response.</typeparam>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="unsuccessfulAction">The unsuccessful action.</param>
        /// <returns>Async take with type T object.</returns>
        Task<T> ExecuteRequestTyped<T>(HttpMethod httpMethod, string url, Action<HttpResponseMessage> unsuccessfulAction = null);

        /// <summary>
        /// Executes the request and returns the typed response body.
        /// </summary>
        /// <typeparam name="T">The object returned from the response.</typeparam>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="content">The content to send with the request.</param>
        /// <param name="unsuccessfulAction">The unsuccessful action.</param>
        /// <returns>Async take with type T object.</returns>
        Task<T> ExecuteRequestTyped<T>(HttpMethod httpMethod, string url, HttpContent content, Action<HttpResponseMessage> unsuccessfulAction = null);

        /// <summary>
        /// Executes the request and returns the typed response body.
        /// </summary>
        /// <typeparam name="T">The object returned from the response.</typeparam>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="headers">List of headers to set on the request.</param>
        /// <param name="content">The content to send with the request.</param>
        /// <param name="unsuccessfulAction">The unsuccessful action.</param>
        /// <returns>Async take with type T object.</returns>
        Task<T> ExecuteRequestTyped<T>(HttpMethod httpMethod, string url, 
            Dictionary<string, string> headers, HttpContent content, Action<HttpResponseMessage> unsuccessfulAction = null);

        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="headers">List of headers to set on the request.</param>
        /// <param name="content">The content to send with the request.</param>
        /// <param name="unsuccessfulAction">The unsuccessful action.</param>
        /// <returns>Async take with HttpResponseMessage.</returns>
        Task<HttpResponseMessage> ExecuteRequest(HttpMethod httpMethod, string url, 
            Dictionary<string, string> headers, HttpContent content, Action<HttpResponseMessage> unsuccessfulAction = null);

        /// <summary>
        /// Executes the request and returns the response.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="content">The content to send with the request.</param>
        /// <param name="unsuccessfulAction">The unsuccessful action.</param>
        /// <returns>Async take with HttpResponseMessage.</returns>
        Task<HttpResponseMessage> ExecuteRequest(HttpMethod httpMethod, string url, HttpContent content, Action<HttpResponseMessage> unsuccessfulAction = null);

        /// <summary>
        /// Executes the request and returns the response.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="unsuccessfulAction">The unsuccessful action.</param>
        /// <returns>Async take with HttpResponseMessage.</returns>
        Task<HttpResponseMessage> ExecuteRequest(HttpMethod httpMethod, string url, Action<HttpResponseMessage> unsuccessfulAction = null);

        /// <summary>
        /// Update the auth token to use the token passed in.
        /// </summary>
        /// <param name="token">String bearer token.</param>
        void SetAuthToken(string token);

        /// <summary>
        /// Update the authentication token to the IAuthentication passed in.
        /// </summary>
        /// <param name="auth">IAuthentication provider.</param>
        void SetAuthToken(IAuthentication auth);
    }
}
