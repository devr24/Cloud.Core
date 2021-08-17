namespace Cloud.Core.Web
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using Cloud.Core.Extensions;
    using Polly;

    /// <summary>
    /// Http Api Service is a wrapper for the HttpClient class.  It handles common HttpClient use cases, such as deserializing json responses.
    /// </summary>
    public class ApiService : IApiService, IDisposable
    {
        private readonly HttpClient _client;
        private string _userBearer;
        private IAuthentication _authentication;

        internal virtual HttpClient Client => _client;

        /// <summary>
        /// Gets a value indicating whether this <see cref="ApiService"/> is disposed.
        /// </summary>
        /// <value><c>true</c> if disposed; otherwise, <c>false</c>.</value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Service that performs http requests using the provided authentication.
        /// </summary>
        public ApiService()
        {
            _client = new HttpClient();
        }

        /// <summary>
        /// Service that performs http requests using the provided authentication.  This constructor can be used with Dependency Injection.
        /// </summary>
        /// <param name="clientFactory">Client factory to get the http client from.</param>
        /// <param name="authentication">Authentication provider for http client.</param>
        public ApiService(IHttpClientFactory clientFactory, IAuthentication authentication)
        {
            _authentication = authentication;
            _client = clientFactory.CreateClient(nameof(ApiService));
        }

        /// <summary>
        /// Service that performs http requests using the provided authentication.  This constructor can be used with Dependency Injection.
        /// </summary>
        /// <param name="clientFactory">Client factory to get the http client from.</param>
        public ApiService(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient(nameof(ApiService));
        }

        /// <summary>
        /// Service that performs http requests using the provided authentication.  This constructor can be used with Dependency Injection.
        /// </summary>
        /// <param name="authToken">Bearer token to access the API being called.</param>
        public ApiService(string authToken)
        {
            _client = new HttpClient();
            _userBearer = authToken;
        }

        /// <summary>
        /// Service that performs http requests using the provided authentication.  This constructor can be used with Dependency Injection.
        /// </summary>
        /// <param name="authentication">Authentication provider for http client.</param>
        public ApiService(IAuthentication authentication)
        {
            _authentication = authentication;
            _client = new HttpClient();
        }

        /// <summary>
        /// Performs a GET request.
        /// </summary>
        /// <typeparam name="T">Type T object to be returned.</typeparam>
        /// <param name="url">Url of GET request.</param>
        /// <param name="unsuccessfulAction">Custom action to perform if request if not successful.</param>
        /// <returns>Async Task with generic object type T.</returns>
        public virtual async Task<T> Get<T>(string url, Action<HttpResponseMessage> unsuccessfulAction = null)
        {
            return await ExecuteRequestTyped<T>(HttpMethod.Get, url, null, null, unsuccessfulAction);
        }

        /// <summary>
        /// Performs a GET request.
        /// </summary>
        /// <typeparam name="T">Type T object to be returned.</typeparam>
        /// <param name="url">Url of GET request.</param>
        /// <param name="headers">Dictionary of headers to append onto the request.</param>
        /// <param name="unsuccessfulAction">Custom action to perform if request if not successful.</param>
        /// <returns>Async Task with generic object type T.</returns>
        public virtual async Task<T> Get<T>(string url, Dictionary<string, string> headers, Action<HttpResponseMessage> unsuccessfulAction = null)
        {
            return await ExecuteRequestTyped<T>(HttpMethod.Get, url, headers, null, unsuccessfulAction);
        }

        /// <summary>
        /// Performs a POST request
        /// </summary>
        /// <typeparam name="T">Type T object to be returned.</typeparam>
        /// <param name="url">Url of POST request.</param>
        /// <param name="content">Content of the POST body.</param>
        /// <param name="unsuccessfulAction">Custom action to perform if request if not successful.</param>
        /// <returns>Async Task with generic object type T.</returns>
        public virtual async Task<T> Post<T>(string url, HttpContent content, Action<HttpResponseMessage> unsuccessfulAction = null)
        {
            return await ExecuteRequestTyped<T>(HttpMethod.Post, url, null, content, unsuccessfulAction);
        }

        /// <summary>
        /// Performs a POST request
        /// </summary>
        /// <typeparam name="T">Type T object to be returned.</typeparam>
        /// <param name="url">Url of POST request.</param>
        /// <param name="headers">Dictionary of headers to append onto the request.</param>
        /// <param name="content">Content of the POST body.</param>
        /// <param name="unsuccessfulAction">Custom action to perform if request if not successful.</param>
        /// <returns>Async Task with generic object type T.</returns>
        public virtual async Task<T> Post<T>(string url, Dictionary<string, string> headers, HttpContent content, Action<HttpResponseMessage> unsuccessfulAction = null)
        {
            return await ExecuteRequestTyped<T>(HttpMethod.Post, url, headers, content, unsuccessfulAction);
        }

        /// <summary>
        /// Performs a POST but with multipart form data.
        /// </summary>
        /// <typeparam name="T">Type T object to be returned.</typeparam>
        /// <param name="url">Url of POST multipart request.</param>
        /// <param name="payload">Mutli-part form data.</param>
        /// <param name="unsuccessfulAction">Custom action to perform if request if not successful.</param>
        /// <returns>Async Task with generic object type T.</returns>
        public virtual async Task<T> PostMultipart<T>(string url, MultipartFormDataContent payload, Action<HttpResponseMessage> unsuccessfulAction = null)
        {
            return await ExecuteRequestTyped<T>(HttpMethod.Post, url, null, payload, unsuccessfulAction);
        }

        /// <summary>
        /// Performs a POST but with multipart form data.
        /// </summary>
        /// <typeparam name="T">Type T object to be returned.</typeparam>
        /// <param name="url">Url of POST multipart request.</param>
        /// <param name="headers">Dictionary of headers to append onto the request.</param>
        /// <param name="payload">Mutli-part form data.</param>
        /// <param name="unsuccessfulAction">Custom action to perform if request if not successful.</param>
        /// <returns>Async Task with generic object type T.</returns>
        public virtual async Task<T> PostMultipart<T>(string url, Dictionary<string, string> headers, MultipartFormDataContent payload, Action<HttpResponseMessage> unsuccessfulAction = null)
        {
            return await ExecuteRequestTyped<T>(HttpMethod.Post, url, headers, payload, unsuccessfulAction);
        }

        /// <summary>
        /// Performs a PUT request.
        /// </summary>
        /// <typeparam name="T">Type T object to be returned.</typeparam>
        /// <param name="url">Url of PUT request.</param>
        /// <param name="content">Content of the POST body</param>
        /// <param name="unsuccessfulAction">Custom action to perform if request if not successful.</param>
        /// <returns>Async Task with generic object type T.</returns>
        public virtual async Task<T> Put<T>(string url, HttpContent content, Action<HttpResponseMessage> unsuccessfulAction = null)
        {
            return await ExecuteRequestTyped<T>(HttpMethod.Put, url, null, content, unsuccessfulAction);
        }

        /// <summary>
        /// Performs a PUT request.
        /// </summary>
        /// <typeparam name="T">Type T object to be returned.</typeparam>
        /// <param name="url">Url of PUT request.</param>
        /// <param name="headers">Dictionary of headers to append onto the request.</param>
        /// <param name="content">Content of the POST body</param>
        /// <param name="unsuccessfulAction">Custom action to perform if request if not successful.</param>
        /// <returns>Async Task with generic object type T.</returns>
        public virtual async Task<T> Put<T>(string url, Dictionary<string, string> headers, HttpContent content, Action<HttpResponseMessage> unsuccessfulAction = null)
        {
            return await ExecuteRequestTyped<T>(HttpMethod.Put, url, headers, content, unsuccessfulAction);
        }

        /// <summary>
        /// Performs a DELETE request.
        /// </summary>
        /// <typeparam name="T">Type T object to be returned.</typeparam>
        /// <param name="url">Url of DELETE request.</param>
        /// <param name="unsuccessfulAction">Custom action to perform if request if not successful.</param>
        /// <returns>Async Task with generic object type T.</returns>
        public virtual async Task<T> Delete<T>(string url, Action<HttpResponseMessage> unsuccessfulAction = null)
        {
            return await ExecuteRequestTyped<T>(HttpMethod.Delete, url, null, null, unsuccessfulAction);
        }

        /// <summary>
        /// Performs a DELETE request.
        /// </summary>
        /// <typeparam name="T">Type T object to be returned.</typeparam>
        /// <param name="url">Url of DELETE request.</param>
        /// <param name="headers">Dictionary of headers to append onto the request.</param>
        /// <param name="unsuccessfulAction">Custom action to perform if request if not successful.</param>
        /// <returns>Async Task with generic object type T.</returns>
        public virtual async Task<T> Delete<T>(string url, Dictionary<string,string> headers, Action<HttpResponseMessage> unsuccessfulAction = null)
        {
            return await ExecuteRequestTyped<T>(HttpMethod.Delete, url, headers, null, unsuccessfulAction);
        }

        /// <summary>
        /// Executes the request and attempts to deserialize the content to an object.
        /// </summary>
        /// <typeparam name="T">Type T object to be returned.</typeparam>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="unsuccessfulAction">The unsuccessful action.</param>
        /// <returns>Deserialized content as object T.</returns>
        /// <exception cref="HttpRequestException">Request to url: {url} failed, Response: {JsonConvert.SerializeObject(response)}</exception>
        public async Task<T> ExecuteRequestTyped<T>(HttpMethod httpMethod, string url, Action<HttpResponseMessage> unsuccessfulAction = null)
        {
            return await ExecuteRequestTyped<T>(httpMethod, url, null, null, unsuccessfulAction);
        }

        /// <summary>
        /// Executes the request and attempts to deserialize the content to an object.
        /// </summary>
        /// <typeparam name="T">Type T object to be returned.</typeparam>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="content">The content.</param>
        /// <param name="unsuccessfulAction">The unsuccessful action.</param>
        /// <returns>Deserialized content as object T.</returns>
        /// <exception cref="HttpRequestException">Request to url: {url} failed, Response: {JsonConvert.SerializeObject(response)}</exception>
        public async Task<T> ExecuteRequestTyped<T>(HttpMethod httpMethod, string url, HttpContent content, Action<HttpResponseMessage> unsuccessfulAction = null)
        {
            return await ExecuteRequestTyped<T>(httpMethod, url, null, content, unsuccessfulAction);
        }

        /// <summary>
        /// Executes the request and attempts to deserialize the content to an object.
        /// </summary>
        /// <typeparam name="T">Type T object to be returned.</typeparam>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="headers">Dictionary of headers to append onto the request.</param>
        /// <param name="content">The content.</param>
        /// <param name="unsuccessfulAction">The unsuccessful action.</param>
        /// <returns>Deserialized content as object T.</returns>
        /// <exception cref="HttpRequestException">Request to url: {url} failed, Response: {JsonConvert.SerializeObject(response)}</exception>
        public async Task<T> ExecuteRequestTyped<T>(HttpMethod httpMethod, string url, Dictionary<string, string> headers, HttpContent content, Action<HttpResponseMessage> unsuccessfulAction = null)
        {
            var response = await Execute(httpMethod, url, headers, content, unsuccessfulAction, true);

            var result = await response.Content.ReadAsStringAsync();

            // If T is a string we just want to return the raw string result.
            if (typeof(T) == typeof(string))
            {
                return (T)(object)result;
            }

            return System.Text.Json.JsonSerializer.Deserialize<T>(result);
        }

        /// <summary>
        /// Executes the request and returns the response.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="headers">Dictionary of headers to append onto the request.</param>
        /// <param name="content">The content.</param>
        /// <param name="unsuccessfulAction">The unsuccessful action.</param>
        /// <returns>HttpResponseMessage with the web response from the request.</returns>
        /// <exception cref="HttpRequestException">Request to url: {url} failed, Response: {JsonConvert.SerializeObject(response)}</exception>
        public Task<HttpResponseMessage> ExecuteRequest(HttpMethod httpMethod, string url, Dictionary<string, string> headers, HttpContent content, Action<HttpResponseMessage> unsuccessfulAction = null)
        {
            return Execute(httpMethod, url, headers, content, unsuccessfulAction);
        }

        /// <summary>
        /// Executes the request and returns the response.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="content">The content.</param>
        /// <param name="unsuccessfulAction">The unsuccessful action.</param>
        /// <returns>HttpResponseMessage with the web response from the request.</returns>
        /// <exception cref="HttpRequestException">Request to url: {url} failed, Response: {JsonConvert.SerializeObject(response)}</exception>
        public async Task<HttpResponseMessage> ExecuteRequest(HttpMethod httpMethod, string url, HttpContent content, Action<HttpResponseMessage> unsuccessfulAction = null)
        {
            return await Execute(httpMethod, url, null, content, unsuccessfulAction);
        }

        /// <summary>
        /// Executes the request and returns the response.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="unsuccessfulAction">The unsuccessful action.</param>
        /// <returns>HttpResponseMessage with the web response from the request.</returns>
        /// <exception cref="HttpRequestException">Request to url: {url} failed, Response: {JsonConvert.SerializeObject(response)}</exception>
        public async Task<HttpResponseMessage> ExecuteRequest(HttpMethod httpMethod, string url, Action<HttpResponseMessage> unsuccessfulAction = null) 
        {
            return await Execute(httpMethod, url, null, null, unsuccessfulAction);
        }

        /// <summary>
        /// Update the auth token to use the token passed in.
        /// </summary>
        /// <param name="token">String bearer token.</param>
        public void SetAuthToken(string token)
        {
            _authentication = null;
            _userBearer = token;
        }

        /// <summary>
        /// Update the authentication token to the IAuthentication passed in.
        /// </summary>
        /// <param name="auth">IAuthentication provider.</param>
        public void SetAuthToken(IAuthentication auth)
        {
            _authentication = auth;
            _userBearer = null;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        #region Internal/private Methods

        /// <summary>
        /// Internal execute http client request method, called by the public api methods.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="headers">Dictionary of headers to append onto the request.</param>
        /// <param name="content">The content.</param>
        /// <param name="unsuccessfulAction">The unsuccessful action.</param>
        /// <param name="jsonCall">Whether or not the response expected will be serialised to json - this effects the headers setup.</param>
        /// <returns>Http response message.</returns>
        private async Task<HttpResponseMessage> Execute(HttpMethod httpMethod, string url, Dictionary<string, string> headers = null, HttpContent content = null, Action<HttpResponseMessage> unsuccessfulAction = null, bool jsonCall = false)
        {
            HttpResponseMessage response = null;

            // Execute http call with a polly return wrapper.
            await Policy.Handle<TimeoutException>()
                .Or<HttpRequestException>()
                .Or<SocketException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(3, retryAttempt))).ExecuteAsync(async () =>
                {
                    // Clear down any existing headers (to ensure no duplication) and add headers for this request.
                    SetupRequestHeaders(headers, jsonCall);

                    switch (httpMethod.Method)
                    {
                        case "GET":
                            response = await Client.GetAsync(url);
                            break;
                        case "POST":
                            response = await Client.PostAsync(url, content);
                            break;
                        case "PUT":
                            response = await Client.PutAsync(url, content);
                            break;
                        case "DELETE":
                            response = await Client.DeleteAsync(url);
                            break;
                    }
                });

            if (!response.IsSuccessStatusCode)
            {
                // If unsuccessful, either execute fallback OR throw an exception (only throw exception if its a json serialization call).
                unsuccessfulAction?.Invoke(response);

                if (jsonCall)
                {
                    throw new HttpRequestException($"Request to url: {url} failed, Response: {System.Text.Json.JsonSerializer.Serialize(response)}");
                }
            }

            return response;
        }

        /// <summary>
        /// Setup request headers by clearing down any existing headers and adding the newly passed in headers collection.
        /// If headers is passed in as null, it will default to use one header only for the Accept header.
        /// Authentication header is also appended here.
        /// </summary>
        /// <param name="headers">Dictionary of headers to add to the request.</param>
        /// <param name="isJsonCall">Whether or not the http call response will be serialized to json - important, as we append the Accept:application/json; header onto the request automatically.</param>
        internal void SetupRequestHeaders(Dictionary<string, string> headers, bool isJsonCall)
        {
            // Headers are used to setup the client request headers.  Make sure an instance exists before manipulating.
            if (headers.IsNullOrDefault())
            {
                headers = new Dictionary<string, string>();
            }

            // If this is a json call but no accept header, then set it.
            if (isJsonCall && !headers.ContainsKey("Accept"))
            {
                headers.Add("Accept", "application/json");
            }
            
            // Remove all existing headers before appeanding new headers
            Client.DefaultRequestHeaders.Accept.Clear();
            foreach (var header in _client.DefaultRequestHeaders)
            {
                Client.DefaultRequestHeaders.Remove(header.Key);
            }

            // Followed by adding all headers in the headers list.
            foreach (var header in headers)
            {
                Client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            // Lastly, set the authorization header if required.
            if (TryGetAccessToken(out string accessToken))
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        /// <summary>
        /// Try to get an access token.
        /// </summary>
        /// <param name="token">Out param which is the generated token (if one was requested).</param>
        /// <returns>True if got an access token and false if not.</returns>
        internal virtual bool TryGetAccessToken(out string token)
        {
            token = null;

            // Use the IAuthentication as first type of authentication and fall back on passed in access token.
            if (_authentication != null)
            {
                token = _authentication.AccessToken.BearerToken;
            }
            else if (_userBearer != null)
            {
                token = _userBearer;
            }

            return !token.IsNullOrEmpty();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                _client?.Dispose();
            }

            Disposed = true;
        }

        #endregion
    }
}
