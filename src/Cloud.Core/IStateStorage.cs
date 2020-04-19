namespace Cloud.Core
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for IStateStorageProvider, used to hook up state storage with the passed in provider.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IStateStorageProvider<T> where T: IStateStorage
    {
        /// <summary>
        /// Gets or sets a single state storage provider.
        /// </summary>
        /// <value>The state storage provider.</value>
        T StateStorageProvider { get; set; }
    }

    /// <summary>
    /// Interface for State Storage. For any store to be considered as state stoage they need to
    /// implement these methods.
    /// </summary>
    public interface IStateStorage
    {
        /// <summary>
        /// Gets the generic state information from storage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        Task<T> GetState<T>(string key);

        /// <summary>
        /// Determines whether [is state stored] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> IsStateStored(string key);

        /// <summary>
        /// Sets the generic object in state storage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="storeObj">The store object.</param>
        /// <returns>Task.</returns>
        Task SetState<T>(string key, T storeObj);
    }
}
