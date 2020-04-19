namespace Cloud.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Entity Full Exception model.
    /// Implements the <see cref="Exception" />
    /// </summary>
    /// <seealso cref="Exception" />
    [Serializable]
    public class EntityFullException : Exception
    {
        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        /// <value>The name of the entity.</value>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the maximum size.
        /// </summary>
        /// <value>The maximum size.</value>
        public long MaxSizeBytes { get; private set; }

        /// <summary>
        /// Gets or sets the size of the current.
        /// </summary>
        /// <value>The size of the current.</value>
        public long CurrentSizeBytes { get; private set; }

        /// <summary>
        /// Gets the percent the entity is used.
        /// </summary>
        /// <value>The percent used.</value>
        public double PercentUsed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFullException"/> class.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        public EntityFullException(string entityName) { EntityName = entityName; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFullException"/> class.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="message">The message.</param>
        public EntityFullException(string entityName, string message) : base(message) { EntityName = entityName; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFullException"/> class.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public EntityFullException(string entityName, string message, Exception innerException) : base(message, innerException) { EntityName = entityName; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFullException" /> class.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="message">The message.</param>
        /// <param name="currentSizeBytes">The current size bytes.</param>
        /// <param name="maxSizeBytes">The maximum size bytes.</param>
        /// <param name="innerException">The inner exception.</param>
        public EntityFullException(string entityName, string message, long currentSizeBytes, long maxSizeBytes,
            Exception innerException) : base(message, innerException)
        {
            EntityName = entityName;
            MaxSizeBytes = maxSizeBytes;
            CurrentSizeBytes = currentSizeBytes;
            PercentUsed = (Convert.ToDouble(currentSizeBytes) / Convert.ToDouble(MaxSizeBytes)) * 100;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictException"/> class.
        /// </summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Streaming context.</param>
        protected EntityFullException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
