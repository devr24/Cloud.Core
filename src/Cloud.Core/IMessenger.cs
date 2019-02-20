namespace Cloud.Core
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    
    /// <summary>
    /// Contract specifying the functionality of message operations like "Complete" and "Abandon".
    /// </summary>
    public interface IMessageOperations
    {
        /// <summary>
        /// Completes the message and removes from the queue.
        /// </summary>
        /// <param name="message">The message we want to complete.</param>
        /// <returns>The async <see cref="Task"/> wrapper</returns>
        Task Complete<T>(T message) where T : class;

        /// <summary>
        /// Abandons a message by returning it to the queue.
        /// </summary>
        /// <param name="message">The message we want to abandon.</param>
        /// <returns>The async <see cref="Task"/> wrapper.</returns>
        Task Abandon<T>(T message) where T : class;

        /// <summary>
        /// Errors a message by moving it specifically to the error queue (dead-letter).
        /// </summary>
        /// <param name="message">The message that we want to move to the error queue.</param>
        /// <returns>The async <see cref="Task"/> wrapper</returns>
        Task Error<T>(T message) where T : class;
    }

    /// <summary>
    /// Contract that provides a simple way to send messages.
    /// </summary>
    /// <inheritdoc cref="System.IDisposable"/>
    public interface ISendMessages : IDisposable
    {
        /// <summary>
        /// Send a message.
        /// </summary>
        /// <typeparam name="T">The type of the message that we are sending.</typeparam>
        /// <param name="message">The message that we are sending.</param>
        /// <returns>The async <see cref="Task"/> wrapper</returns>
        Task Send<T>([NotNull] T message) where T : class;

        /// <summary>
        /// Send a batch of messages.
        /// </summary>
        /// <typeparam name="T">The type of the message that we are sending.</typeparam>
        /// <param name="messages">IList of messages to send.</param>
        /// <param name="batchSize">Size of the message batch to send.</param>
        /// <returns>The async <see cref="Task"/> wrapper.</returns>
        Task SendBatch<T>([NotNull] IList<T> messages, int batchSize = 10) where T : class;
    }

    /// <summary>
    /// Contract that provides a simple way to receive messages through callbacks.
    /// </summary>
    /// <inheritdoc cref="Cloud.Core.ISendMessages"/>
    /// <inheritdoc cref="Cloud.Core.IMessageOperations"/>
    public interface IMessenger : ISendMessages, IMessageOperations
    {
        /// <summary>
        /// Sets up a call back for receiving any message of type <typeparamref name="T" />.
        /// If you try to setup more then one callback to the same message type <typeparamref name="T" /> you'll get an <see cref="InvalidOperationException" />.
        /// </summary>
        /// <typeparam name="T">The type of the message that we are subscribing to.</typeparam>
        /// <param name="successCallback">The <see cref="Action{T}" /> delegate that will be called for each message received.</param>
        /// <param name="errorCallback">The <see cref="Action{Exception}" /> delegate that will be called when an error occurs.</param>
        /// <param name="batchSize">The size of the batch when reading for a queue.</param>
        /// <exception cref="InvalidOperationException">Thrown when you attempt to setup multiple callbacks against the same <typeparamref name="T" /> parameter.</exception>
        void Receive<T>([NotNull] Action<T> successCallback, [CanBeNull] Action<Exception> errorCallback = null, int batchSize = 10) where T : class;
        
        /// <summary>
        /// Gets a message, for the given message type, and returns an object <see cref="IMessageItem{T}"/> which gives details of the received message.
        /// </summary>
        /// <typeparam name="T">The type of the message returned.</typeparam>
        /// <returns>The typed <see cref="IMessageItem{T}"/>.</returns>
        IMessageItem<T> ReceiveOne<T>() where T : class;
        
        /// <summary>
        /// Stop receiving a message type.
        /// </summary>
        /// <typeparam name="T">The type of the message that we are cancelling the receive on.</typeparam>
        void CancelReceive<T>() where T : class;

        /// <summary>
        /// Gets the receiver entity message count.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Task{System.Int64} total message count for entity.</returns>
        Task<long> GetReceiverMessageCount<T>() where T : class;
    }

    /// <summary>
    /// Contract for Receiving a message.
    /// </summary>
    public interface IMessageItem<T>
    {
        /// <summary>
        /// Gets the exception thrown while attempting to Receive a message.
        /// </summary>
        /// <value>
        /// The exception thrown.
        /// </value>
        Exception Error { get; set; }

        /// <summary>
        /// Gets the body of the received message.
        /// </summary>
        /// <value>
        /// The body of the received message, returns null when there is no message received.
        /// </value>
        T Body { get; set; }
    }

    /// <summary>
    /// Contract that allows a reactive way to receive messages.
    /// </summary>
    /// <inheritdoc cref="Cloud.Core.ISendMessages"/>
    /// <inheritdoc cref="Cloud.Core.IMessageOperations"/>
    public interface IReactiveMessenger : ISendMessages, IMessageOperations
    {
        /// <summary>
        /// Set up the required receive pipeline, for the given message type, and return a reactive <see cref="IObservable{T}"/> that you can subscribe to.
        /// </summary>
        /// <typeparam name="T">The type of the message returned by the observable.</typeparam>
        /// <param name="batchSize">The size of the batch when reading from a queue.</param>
        /// <returns>The typed <see cref="IObservable{T}"/> that you subscribed to.</returns>
        IObservable<T> StartReceive<T>(int batchSize = 10) where T : class;

        /// <summary>
        /// Gets a message, for the given message type, and returns an object <see cref="IMessageItem{T}"/> which gives details of the received message.
        /// </summary>
        /// <typeparam name="T">The type of the message returned.</typeparam>
        /// <returns>The typed <see cref="IMessageItem{T}"/>.</returns>
        IMessageItem<T> ReceiveOne<T>() where T : class;

        /// <summary>
        /// Stop receiving a message type.
        /// </summary>
        /// <typeparam name="T">The type of the message that we are cancelling the receive on.</typeparam>
        void CancelReceive<T>() where T : class;

        /// <summary>
        /// Gets the receiver entity message count.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Task{System.Int64} total message count for entity.</returns>
        Task<long> GetReceiverMessageCount<T>() where T : class;
    }
}

