namespace Cloud.Core
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Interface ITelemetryLogger with type
    /// Implements the <see cref="Cloud.Core.ITelemetryLogger" />
    /// </summary>
    /// <typeparam name="T">Type of TelemetryLogger.</typeparam>
    /// <seealso cref="Cloud.Core.ITelemetryLogger" />
    public interface ITelemetryLogger<T> : ITelemetryLogger { }

    /// <summary>
    /// Contract for all implementations of the logger.  
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Logging.ILogger" />
    public interface ITelemetryLogger : ILogger
    {
        /// <summary>
        /// Logs verbose messages.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="properties">The properties to concatenate together and add to the message.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogVerbose(string message, Dictionary<string, string> properties = null,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Logs verbose message, with an object which is serialized and logged.
        /// </summary>
        /// <typeparam name="T">Type of object to log.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="objectToLog">Object instance to serialize and log.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogVerbose<T>(string message, T objectToLog,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Logs information message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="properties">The properties to concatenate together and add to the message.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogInformation(string message, Dictionary<string, string> properties = null,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Logs information message, with an object which is serialized and logged.
        /// </summary>
        /// <typeparam name="T">Type of object to log.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="objectToLog">Object instance to serialize and log.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogInformation<T>(string message, T objectToLog,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Logs critical message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="properties">The properties to concatenate together and add to the message.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogCritical(string message, Dictionary<string, string> properties = null,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Logs critical message, with an object which is serialized and logged.
        /// </summary>
        /// <typeparam name="T">Type of object to log.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="objectToLog">Object instance to serialize and log.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogCritical<T>(string message, T objectToLog,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Logs critical exception.
        /// </summary>
        /// <param name="ex">The exception to log as critical.</param>
        /// <param name="properties">The properties to concatenate together and add to the message.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogCritical(Exception ex, Dictionary<string, string> properties = null,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Logs critical exception, with an object which is serialized and logged.
        /// </summary>
        /// <typeparam name="T">Type of object to log.</typeparam>
        /// <param name="ex">The exception to log as critical.</param>
        /// <param name="objectToLog">Object instance to serialize and log.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogCritical<T>(Exception ex, T objectToLog,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Logs warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="properties">The properties to concatenate together and add to the message.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogWarning(string message, Dictionary<string, string> properties = null,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Logs warning message, with an object which is serialized and logged.
        /// </summary>
        /// <typeparam name="T">Type of object to log.</typeparam>
        /// <param name="message">The message to log.</param>
        /// <param name="objectToLog">Object instance to serialize and log.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogWarning<T>(string message, T objectToLog,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Logs warning exception.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <param name="properties">The properties to concatenate together and add to the message.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogWarning(Exception ex, Dictionary<string, string> properties = null,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Logs warning exception, with an object which is serialized and logged.
        /// </summary>
        /// <typeparam name="T">Type of object to log.</typeparam>
        /// <param name="ex">The exception to log.</param>
        /// <param name="objectToLog">Object instance to serialize and log.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogWarning<T>(Exception ex, T objectToLog,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Logs error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="properties">The properties to concatenate together and add to the message.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogError(string message, Dictionary<string, string> properties = null,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Logs error exception.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <param name="properties">The properties to output.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogError(Exception ex, Dictionary<string, string> properties = null,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Logs error exception, with an object which is serialized and logged.
        /// </summary>
        /// <typeparam name="T">Type of object to log.</typeparam>
        /// <param name="ex">The exception to log.</param>
        /// <param name="objectToLog">Object instance to serialize and log.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogError<T>(Exception ex, T objectToLog,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Logs error exception.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="properties">The properties to concatenate together and add to the message.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogError(Exception ex, string message, Dictionary<string, string> properties = null,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Logs error exception, with an object which is serialized and logged.
        /// </summary>
        /// <typeparam name="T">Type of object to log.</typeparam>
        /// <param name="ex">The exception to log.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="objectToLog">Object instance to serialize and log.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogError<T>(Exception ex, string message, T objectToLog,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Logs the metric value.
        /// </summary>
        /// <param name="metricName">Name of the metric to log.</param>
        /// <param name="metricValue">The metric value.</param>
        /// <param name="properties">The properties to output.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogMetric(string metricName, double metricValue, Dictionary<string, string> properties,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Logs the metric value, with an object which is serialized and logged.
        /// </summary>
        /// <typeparam name="T">Type of object to log.</typeparam>
        /// <param name="metricName">Name of the metric to log.</param>
        /// <param name="metricValue">The metric value.</param>
        /// <param name="objectToLog">Object instance to serialize and log.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogMetric<T>(string metricName, double metricValue, T objectToLog,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Logs the metric value.
        /// </summary>
        /// <param name="metricName">Name of the metric to log.</param>
        /// <param name="metricValue">The metric value.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        void LogMetric(string metricName, double metricValue,
            [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Flush the logs to the sink.
        /// </summary>
        void Flush();
    }
}
