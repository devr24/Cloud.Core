namespace Cloud.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Entity Disabled Exception model.
    /// Implements the <see cref="Exception" />
    /// </summary>
    /// <seealso cref="Exception" />
    [Serializable]
    public class EntityDisabledException : Exception
    {
        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        /// <value>The name of the entity.</value>
        public string EntityName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDisabledException"/> class.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        public EntityDisabledException(string entityName) { EntityName = entityName; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDisabledException"/> class.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="message">The message.</param>
        public EntityDisabledException(string entityName, string message) : base(message) { EntityName = entityName; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDisabledException"/> class.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public EntityDisabledException(string entityName, string message, Exception innerException) : base(message, innerException) { EntityName = entityName; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictException"/> class.
        /// </summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Streaming context.</param>
        protected EntityDisabledException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
