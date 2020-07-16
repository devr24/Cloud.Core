namespace Cloud.Core.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Conflict Exception model.
    /// Implements the <see cref="Exception" />
    /// </summary>
    /// <seealso cref="Exception" />
    [Serializable]
    public class TemplateMappingException : Exception
    {
        /// <summary>Name of the template requested.</summary>
        public string TemplateId { get; }

        /// <summary>Whether the template requested was actually found <c>true</c> or not <c>false</c>.</summary>
        public bool TemplateFound { get; }

        /// <summary>The list of Keys found on the template.</summary>
        public List<string> TemplateKeys { get; }

        /// <summary>The list of keys (and their values) belonging to the model sent in.</summary>
        public Dictionary<string, string> ModelKeyValues { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateMappingException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="templateId">Name of the template requested.</param>
        /// <param name="templateFound">Whether the template was found (<c>true</c> if [template found], otherwise <c>false</c>).</param>
        /// <param name="templateKeys">Template keys found.</param>
        /// <param name="modelKeyValues">The model keys and values.</param>
        public TemplateMappingException(string message, string templateId, bool templateFound, List<string> templateKeys, Dictionary<string, string> modelKeyValues) : base(message) 
        {
            TemplateId = templateId;
            TemplateFound = templateFound;
            TemplateKeys = templateKeys;
            ModelKeyValues = modelKeyValues;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateMappingException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        /// <param name="templateId">Name of the template requested.</param>
        /// <param name="templateFound">Whether the template was found (<c>true</c> if [template found], otherwise <c>false</c>).</param>
        /// <param name="templateKeys">Template keys found.</param>
        /// <param name="modelKeyValues">The model keys and values.</param>
        public TemplateMappingException(string message, Exception innerException, string templateId, bool templateFound, List<string> templateKeys, Dictionary<string, string> modelKeyValues) : base(message, innerException)
        {
            TemplateId = templateId;
            TemplateFound = templateFound;
            TemplateKeys = templateKeys;
            ModelKeyValues = modelKeyValues;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateMappingException"/> class.
        /// </summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Streaming context.</param>
        protected TemplateMappingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
