namespace Cloud.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    /// <summary>
    /// Contract specifying the functionality of message operations like "Complete" and "Abandon".
    /// </summary>
    public interface IMessageOperations : INamedInstance
    {
        /// <summary>
        /// Gets a single message of type T.
        /// </summary>
        /// <typeparam name="T">The type of the message returned.</typeparam>
        /// <returns>The typed T.</returns>
        T ReceiveOne<T>() where T : class;

        /// <summary>
        /// Gets a single message with IMessageEntity wrapper.
        /// </summary>
        /// <typeparam name="T">Type of message entity body.</typeparam>
        /// <returns>IMessageEntity wrapper with body and properties.</returns>
        IMessageEntity<T> ReceiveOneEntity<T>() where T : class;

        /// <summary>
        /// Read a batch of typed messages in a synchronous manner.
        /// </summary>
        /// <typeparam name="T">Type of object on the entity.</typeparam>
        /// <param name="batchSize">Size of the batch.</param>
        /// <returns>IMessageItem&lt;T&gt;.</returns>
        Task<List<T>> ReceiveBatch<T>(int batchSize) where T : class;

        /// <summary>
        /// Receives a batch of message in a synchronous manner of type IMessageEntity types.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="batchSize">Size of the batch.</param>
        /// <returns>IMessageEntity&lt;T&gt;.</returns>
        Task<List<IMessageEntity<T>>> ReceiveBatchEntity<T>(int batchSize) where T : class;

        /// <summary>
        /// Read additional properties from a message.
        /// </summary>
        /// <typeparam name="T">Type T of message.</typeparam>
        /// <param name="msg">Message body, used to identity the message to read from.</param>
        /// <returns>Dictionary of string, object properties.</returns>
        IDictionary<string, object> ReadProperties<T>(T msg) where T : class;

        /// <summary>
        /// Completes the message and removes from the queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message we want to complete.</param>
        /// <returns>The async <see cref="Task" /> wrapper</returns>
        Task Complete<T>(T message) where T : class;

        /// <summary>
        /// Completes a list of messages.
        /// </summary>
        /// <typeparam name="T">Type of messages to complete</typeparam>
        /// <param name="message">The message list.</param>
        /// <returns></returns>
        Task CompleteAll<T>(IEnumerable<T> message) where T : class;

        /// <summary>
        /// Abandons a message by returning it to the queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message we want to abandon.</param>
        /// <param name="propertiesToModify">The message properties to modify on abandon.</param>
        /// <returns>The async <see cref="Task" /> wrapper.</returns>
        Task Abandon<T>(T message, KeyValuePair<string, object>[] propertiesToModify = null) where T : class;

        /// <summary>
        /// Defers a message in the the queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message we want to abandon.</param>
        /// <param name="propertiesToModify">The message properties to modify on abandon.</param>
        /// <returns>The async <see cref="Task" /> wrapper.</returns>
        Task Defer<T>(T message, KeyValuePair<string, object>[] propertiesToModify = null) where T : class;

        /// <summary>
        /// Receives a batch of deferred messages of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identities">The list of identities pertaining to the batch.</param>
        /// <returns>IMessageItem&lt;T&gt;.</returns>
        Task<List<T>> ReceiveDeferredBatch<T>(IEnumerable<long> identities) where T : class;

        /// <summary>
        /// Receives a batch of deferred messages of type IMessageEntity types.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identities">The list of identities pertaining to the batch</param>
        /// <returns>IMessageEntity&lt;T&gt;.</returns>
        Task<List<IMessageEntity<T>>> ReceiveDeferredBatchEntity<T>(IEnumerable<long> identities) where T : class;

        /// <summary>
        /// Errors a message by moving it specifically to the error queue (dead-letter).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message that we want to move to the error queue.</param>
        /// <param name="reason">(optional) The reason for erroring the message.</param>
        /// <returns>The async <see cref="Task" /> wrapper</returns>
        Task Error<T>(T message, string reason = null) where T : class;

        /// <summary>
        /// Gets a signed access URL to the current messenger.
        /// </summary>
        /// <param name="accessConfig">Access config including required permissions and Access URL Expiry.</param>
        /// <returns>The signed access URL for the specified messenger.</returns>
        string GetSignedAccessUrl(ISignedAccessConfig accessConfig);
    }

    /// <summary>
    /// Interface IMessageEntityManager - used for managing message entities.
    /// Extend this interface to add new manager orientated methods such as create or delete entities to read or send to.
    /// </summary>
    public interface IMessageEntityManager
    {
        /// <summary>
        /// Gets the receiver entity usage percentage. 1.0 represents 100%.
        /// </summary>
        /// <returns>Task&lt;System.Double&gt;.</returns>
        Task<decimal> GetReceiverEntityUsagePercentage();

        /// <summary>
        /// Gets the sender entity usage percentage. 1.0 represents 100%.
        /// </summary>
        /// <returns>Task&lt;System.Double&gt;.</returns>
        Task<decimal> GetSenderEntityUsagePercentage();

        /// <summary>
        /// Determines whether [receiver entity is disabled].
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> IsReceiverEntityDisabled();

        /// <summary>
        /// Determines whether [sender entity is disabled].
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> IsSenderEntityDisabled();

        /// <summary>
        /// Gets the receiver active and errored message count.
        /// </summary>
        /// <returns>Task&lt;EntityMessageCount&gt;.</returns>
        Task<EntityMessageCount> GetReceiverMessageCount();

        /// <summary>
        /// Gets the sender active and errored message count.
        /// </summary>
        /// <returns>Task&lt;EntityMessageCount&gt;.</returns>
        Task<EntityMessageCount> GetSenderMessageCount();

        /// <summary>
        /// Creates the entity.
        /// </summary>
        Task CreateEntity(IEntityConfig config);

        /// <summary>
        /// Check if the entity exists.
        /// </summary>
        /// <param name="entityName">Name of the entity to check exists.</param>
        /// <returns>Task.</returns>
        Task<bool> EntityExists(string entityName);

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="entityName">Name of the entity to delete.</param>
        /// <returns>Task.</returns>
        Task DeleteEntity(string entityName);
    }

    /// <summary>
    /// Model containing active and error message counters.
    /// </summary>
    public class EntityMessageCount
    {
        /// <summary>
        /// Gets or sets the entity errored message count.
        /// </summary>
        /// <value>The errored message count.</value>
        public long ErroredEntityCount { get; set; } = -1;

        /// <summary>
        /// Gets or sets the entity active message count.
        /// </summary>
        /// <value>The active message count.</value>
        public long ActiveEntityCount { get; set; } = -1;
    }

    /// <summary>
    /// Contract that provides a simple way to send messages.
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface ISendMessages : IDisposable
    {
        /// <summary>
        /// Send a message.
        /// </summary>
        /// <typeparam name="T">The type of the message that we are sending.</typeparam>
        /// <param name="message">The message body that we are sending.</param>
        /// <param name="properties">Any additional message properties to add.</param>
        /// <returns>The async <see cref="Task" /> wrapper</returns>
        Task Send<T>([NotNull] T message, KeyValuePair<string, object>[] properties = null) where T : class;

        /// <summary>
        /// Send a batch of messages.
        /// </summary>
        /// <typeparam name="T">The type of the message that we are sending.</typeparam>
        /// <param name="messages">IEnumerable of messages to send.</param>
        /// <param name="batchSize">Size of the message batch to send.</param>
        /// <returns>The async <see cref="Task" /> wrapper.</returns>
        Task SendBatch<T>([NotNull] IEnumerable<T> messages, int batchSize = 10) where T : class;

        /// <summary>
        /// Sends the batch with properties.
        /// </summary>
        /// <typeparam name="T">The type of the message that we are sending.</typeparam>
        /// <param name="messages">The messages to send.</param>
        /// <param name="properties">The properties to associate with ALL messages.</param>
        /// <param name="batchSize">Size of each batch.</param>
        /// <returns>Task.</returns>
        Task SendBatch<T>(IEnumerable<T> messages, KeyValuePair<string, object>[] properties, int batchSize = 100) where T : class;

        /// <summary>
        /// Sends the batch with a function to set the properties based on the message.
        /// </summary>
        /// <typeparam name="T">The type of the message that we are sending.</typeparam>
        /// <param name="messages">The messages to send.</param>
        /// <param name="setProps">The function to set props for each message.</param>
        /// <param name="batchSize">Size of each batch.</param>
        /// <returns>Task.</returns>
        Task SendBatch<T>(IEnumerable<T> messages, Func<T, KeyValuePair<string, object>[]> setProps, int batchSize = 100) where T : class;
    }

    /// <summary>
    /// Contract that provides a simple way to receive messages through callbacks.
    /// Implements the <see cref="ISendMessages" />
    /// Implements the <see cref="IMessageOperations" />
    /// </summary>
    /// <seealso cref="Cloud.Core.ISendMessages" />
    /// <seealso cref="Cloud.Core.IMessageOperations" />
    public interface IMessenger : ISendMessages, IMessageOperations
    {
        /// <summary>
        /// Gets the messaging entity manager.
        /// </summary>
        /// <value>The entity manager.</value>
        /// <seealso cref="IMessageEntityManager" />
        IMessageEntityManager EntityManager { get; }

        /// <summary>
        /// Sets up a call back for receiving any message of type <typeparamref name="T" />.
        /// If you try to setup more then one callback to the same message type <typeparamref name="T" /> you'll get an <see cref="InvalidOperationException" />.
        /// </summary>
        /// <typeparam name="T">The type of the message that we are subscribing to.</typeparam>
        /// <param name="successCallback">The <see cref="Action{T}" /> delegate that will be called for each message received.</param>
        /// <param name="errorCallback">The <see cref="Action{Exception}" /> delegate that will be called when an error occurs.</param>
        /// <param name="batchSize">The size of the batch when reading for a queue.</param>
        /// <exception cref="InvalidOperationException">Thrown when you attempt to setup multiple callbacks against the same <typeparamref name="T" /> parameter.</exception>
        void Receive<T>([NotNull] Action<T> successCallback, [MaybeNull] Action<Exception> errorCallback, int batchSize = 10) where T : class;

        /// <summary>
        /// Stop receiving a message type.
        /// </summary>
        /// <typeparam name="T">The type of the message that we are cancelling the receive on.</typeparam>
        void CancelReceive<T>() where T : class;
    }

    /// <summary>
    /// Contract that allows a reactive way to receive messages.
    /// Implements the <see cref="ISendMessages" />
    /// Implements the <see cref="IMessageOperations" />
    /// </summary>
    /// <seealso cref="ISendMessages" />
    /// <seealso cref="IMessageOperations" />
    public interface IReactiveMessenger : ISendMessages, IMessageOperations
    {
        /// <summary>
        /// Gets the entity manager.
        /// </summary>
        /// <value>The entity manager.</value>
        IMessageEntityManager EntityManager { get; }

        /// <summary>
        /// Set up the required receive pipeline, for the given message type, and return a reactive <see cref="IObservable{T}" /> that you can subscribe to.
        /// </summary>
        /// <typeparam name="T">The type of the message returned by the observable.</typeparam>
        /// <param name="batchSize">The size of the batch when reading from a queue.</param>
        /// <returns>The typed <see cref="IObservable{T}" /> that you subscribed to.</returns>
        IObservable<T> StartReceive<T>(int batchSize = 10) where T : class;

        /// <summary>
        /// Stop receiving a message type.
        /// </summary>
        /// <typeparam name="T">The type of the message that we are cancelling the receive on.</typeparam>
        void CancelReceive<T>() where T : class;

        /// <summary>
        /// Update the receiver details
        /// </summary>
        /// <param name="entityName">The name of the entity to listen to.</param>
        /// <param name="entityDeadletterName">Name of the entity dead-letter.</param>
        /// <param name="entitySubscriptionName">The name of the subscription on the entity to listen to.</param>
        /// <param name="createIfNotExists">The resource will be created if it does not exist.</param>
        /// <param name="entityFilter">A filter that will be applied to the entity if created through this method.</param>
        /// <returns>Task.</returns>
        Task UpdateReceiver(string entityName, string entitySubscriptionName = null, bool createIfNotExists = false, KeyValuePair<string, string>? entityFilter = null, string entityDeadletterName = null);
    }

    /// <summary>
    /// Message wrapper entity, that has a body and list of properties.
    /// </summary>
    /// <typeparam name="T">Type of message entity body.</typeparam>
    public interface IMessageEntity<T> where T : class
    {
        /// <summary>
        /// Entity message body.
        /// </summary>
        T Body { get; set; }

        /// <summary>
        /// Collection of properties for the entity message.
        /// </summary>
        IDictionary<string, object> Properties { get; set; }

        /// <summary>
        /// Gets the message properties as a concrete type.
        /// </summary>
        /// <typeparam name="O">Type of object to return when mapping the properties.</typeparam>
        /// <returns>Concrete type TO, representing the message.</returns>
        O GetPropertiesTyped<O>() where O : class, new();
    }
}

