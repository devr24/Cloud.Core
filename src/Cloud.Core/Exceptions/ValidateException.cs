namespace Cloud.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using Validation;

    /// <summary>
    /// Class Validate exception.
    /// Implements the <see cref="System.Exception" />
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class ValidateException : Exception
    {
        /// <summary>
        /// Gets the error collection.
        /// </summary>
        /// <value>The errors.</value>
        public IEnumerable<ValidationResult> Errors { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateException"/> class.
        /// </summary>
        public ValidateException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateException"/> class.
        /// </summary>
        /// <param name="result">The validation result to build from.</param>
        public ValidateException([NotNull]ValidateResult result) : base("Validation failed")
        {
            Errors = result.Errors;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ValidateException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ValidateException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateException"/> class.
        /// </summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Streaming context.</param>
        protected ValidateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
