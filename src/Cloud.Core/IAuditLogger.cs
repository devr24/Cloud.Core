namespace Cloud.Core
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for audit logging.
    /// </summary>
    public interface IAuditLogger
    {
        /// <summary>
        /// Writes an audit log message (only).  The minimum information that can be logged.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="message">The audit message to log.</param>
        /// <returns>Task.</returns>
        Task WriteLog(string eventName, string message);

        /// <summary>
        /// Writes an audit log message with the source and currentValue.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="message">The audit message of the log.</param>
        /// <param name="source">The source object name that has changed.</param>
        /// <param name="currentValue">The current value of the object.</param>
        /// <returns>Task.</returns>
        Task WriteLog(string eventName, string message, string source, object currentValue);

        /// <summary>
        /// Writes an audit log message, with the source and values (previous/new).
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="message">The audit message to log.</param>
        /// <param name="source">The source object name that has changed.</param>
        /// <param name="previousValue">The previous value of the object.</param>
        /// <param name="currentValue">The current value of the object.</param>
        /// <returns>Task.</returns>
        Task WriteLog(string eventName, string message, string source, object previousValue, object currentValue);

        /// <summary>
        /// Writes an audit log message, capturing the UserIdentifier of the account making the change.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="message">The audit message to log.</param>
        /// <param name="userIdentifier">The user identifier of the account making the change.</param>
        /// <returns>Task.</returns>
        Task WriteLog(string eventName, string message, string userIdentifier);

        /// <summary>
        /// Writes an audit log message with User
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="message">The audit message to log.</param>
        /// <param name="userIdentifier">The user identifier of the account making the change.</param>
        /// <param name="source">The source object.</param>
        /// <param name="currentValue">The current value.</param>
        /// <returns>Task.</returns>
        Task WriteLog(string eventName, string message, string userIdentifier, string source, object currentValue);

        /// <summary>
        /// Writes an audit log message with UserIdentifier, source object name, previous and new value of the object.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="message">The audit message to log.</param>
        /// <param name="userIdentifier">The user identifier of the account making the change.</param>
        /// <param name="source">The source object.</param>
        /// <param name="previousValue">The previous value.</param>
        /// <param name="currentValue">The current value.</param>
        /// <returns>Task.</returns>
        Task WriteLog(string eventName, string message, string userIdentifier, string source, object previousValue, object currentValue);
    }
}
