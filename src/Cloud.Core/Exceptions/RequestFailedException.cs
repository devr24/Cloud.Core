namespace Cloud.Core.Exceptions
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;

    /// <summary>
    /// Request Failed Exception model.
    /// Implements the <see cref="Exception" />
    /// </summary>
    /// <seealso cref="Exception" />
    [Serializable]
    public class RequestFailedException<T> : Exception
    {
        /// <summary>Http response status code.</summary>
        public HttpStatusCode ResponseStatusCode { get; }

        /// <summary>Response body.</summary>
        public string ResponseBody { get; }

        /// <summary>Request object.</summary>
        public T RequestObject { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestFailedException{T}"/> class.
        /// </summary>
        /// <param name="responseStatus">The response http status.</param>
        /// <param name="responseBody">The response body.</param>
        /// <param name="requestObject">The request object.</param>
        public RequestFailedException(HttpStatusCode responseStatus, string responseBody, T requestObject)
        {
            ResponseStatusCode = responseStatus;
            ResponseBody = responseBody;
            RequestObject = requestObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestFailedException{T}"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="responseStatus">The response http status.</param>
        /// <param name="responseBody">The response body.</param>
        /// <param name="requestObject">The request object.</param>
        public RequestFailedException(string message, HttpStatusCode responseStatus, string responseBody, T requestObject) : base(message) 
        {
            ResponseStatusCode = responseStatus;
            ResponseBody = responseBody;
            RequestObject = requestObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestFailedException{T}"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        /// <param name="responseStatus">The response http status.</param>
        /// <param name="responseBody">The response body.</param>
        /// <param name="requestObject">The request object.</param>
        public RequestFailedException(string message, Exception innerException, HttpStatusCode responseStatus, string responseBody, T requestObject) : base(message, innerException)
        {
            ResponseStatusCode = responseStatus;
            ResponseBody = responseBody;
            RequestObject = requestObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestFailedException{T}"/>"/> class.
        /// </summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Streaming context.</param>
        protected RequestFailedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
