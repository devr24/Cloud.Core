namespace Cloud.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Contract provides functions for table stores.
    /// Implements the <see cref="IStateStorage" /> interface, making the implementors of this interface eligable
    /// to be used as state storage mechanisms.
    /// </summary>
    /// <seealso cref="IStateStorage" />
    /// <seealso cref="IDisposable" />
    public interface ITableStorage : INamedInstance
    {
        /// <summary>
        /// Gets the data table entity.
        /// </summary>
        /// <typeparam name="T">Type of entity being returned.</typeparam>
        /// <param name="tableName">Name of the table within the store.</param>
        /// <param name="key">The key to find the individual record.</param>
        /// <returns><see cref="Task{T}" /> with object found.</returns>
        Task<T> GetEntity<T>(string tableName, string key) where T : class, ITableItem, new();

        /// <summary>
        /// Checks to see if a data entity with the key exists for the specified table name.
        /// </summary>
        /// <param name="tableName">Name of the table to query.</param>
        /// <param name="key">The key to identity the record being searched for.</param>
        /// <returns><see cref="Task" /> of <see cref="bool" /> where [true] means the record exists and [false] shows it does not.</returns>
        Task<bool> Exists(string tableName, string key);

        /// <summary>
        /// Deletes the entity for the table store.
        /// </summary>
        /// <param name="tableName">Name of the table to query.</param>
        /// <param name="key">The key to find the record for deletion.</param>
        /// <returns><see cref="Task" />.</returns>
        Task DeleteEntity(string tableName, string key);

        /// <summary>
        /// Deletes multiple entities.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="keys">The keys for deletion.</param>
        /// <param name="batchSize">Size of the batch (defaults to 10).</param>
        /// <returns>Task.</returns>
        Task DeleteEntities(string tableName, List<string> keys, int batchSize = 10);

        /// <summary>
        /// Upserts (adds if does not exist or updates if already exists) the entity.
        /// </summary>
        /// <typeparam name="T">Type of data entity being updated.</typeparam>
        /// <param name="tableName">Name of the table to search within.</param>
        /// <param name="data">The data to store.</param>
        /// <returns><see cref="Task" />.</returns>
        Task UpsertEntity<T>(string tableName, T data) where T : class, ITableItem;

        /// <summary>
        /// Upserts multiple entities.
        /// </summary>
        /// <typeparam name="T">List of entities to insert or update.</typeparam>
        /// <param name="tableName">Name of the table the items will be added to.</param>
        /// <param name="data">The data.</param>
        /// <param name="batchSize">Size of the batch (defaults to 10).</param>
        /// <returns>Task.</returns>
        Task UpsertEntities<T>(string tableName, List<T> data, int batchSize = 10) where T : class, ITableItem;

        /// <summary>
        /// List the entities of a given table name, with a supplied query.  Results returned as an Enumerable.
        /// </summary>
        /// <typeparam name="T">Type of object returned in the Enumerable.</typeparam>
        /// <param name="tableName">Name of the table to search within.</param>
        /// <param name="selectColumns">The columns to select (if required).</param>
        /// <param name="filterQuery">The query to execute.</param>
        /// <param name="token">Cancellation token source.</param>
        /// <returns>Returns enumerable list.</returns>
        IEnumerable<T> ListEntities<T>(string tableName, List<string> selectColumns, string filterQuery, CancellationTokenSource token = default)
            where T : class, ITableItem, new();

        /// <summary>
        /// List the entities of a given table name, with a supplied query.  Results returned as an Enumerable.
        /// </summary>
        /// <typeparam name="T">Type of object returned in the Enumerable.</typeparam>
        /// <param name="tableName">Name of the table to search within.</param>
        /// <param name="selectColumns">The columns to select (if required).</param>
        /// <param name="token">Cancellation token source.</param>
        /// <returns>Returns enumerable list.</returns>
        IEnumerable<T> ListEntities<T>(string tableName, List<string> selectColumns, CancellationTokenSource token = default)
            where T : class, ITableItem, new();

        /// <summary>
        /// List the entities of a given table name, with a supplied query.  Results returned as an Enumerable.
        /// </summary>
        /// <typeparam name="T">Type of object returned in the Enumerable.</typeparam>
        /// <param name="tableName">Name of the table to search within.</param>
        /// <param name="filterQuery">The query to execute.</param>
        /// <param name="token">Cancellation token source.</param>
        /// <returns>Returns enumerable list.</returns>
        IEnumerable<T> ListEntities<T>(string tableName, string filterQuery, CancellationTokenSource token = default)
            where T : class, ITableItem, new();

        /// <summary>
        /// List the entities of a given table name, with a supplied query.  Results returned as an Enumerable.
        /// </summary>
        /// <typeparam name="T">Type of object returned in the Enumerable.</typeparam>
        /// <param name="tableName">Name of the table to search within.</param>
        /// <param name="token">Cancellation token source.</param>
        /// <returns>Returns enumerable list.</returns>
        IEnumerable<T> ListEntities<T>(string tableName, CancellationTokenSource token = default)
            where T : class, ITableItem, new();

        /// <summary>
        /// List the entities of a given table name, with a supplied query.  Results returned as an Enumerable.
        /// </summary>
        /// <typeparam name="T">Type of object returned in the Enumerable.</typeparam>
        /// <param name="tableName">Name of the table to search within.</param>
        /// <param name="token">Cancellation token source.</param>
        /// <returns>Returns enumerable list.</returns>
        IObservable<T> ListEntitiesObservable<T>(string tableName, CancellationTokenSource token = default)
            where T : class, ITableItem, new();

        /// <summary>
        /// List the entities of a given table name, with a supplied query.  Results returned as an Observable.
        /// </summary>
        /// <typeparam name="T">Type of object returned in the Enumerable.</typeparam>
        /// <param name="tableName">Name of the table to search within.</param>
        /// <param name="filterQuery">The query to execute.</param>
        /// <param name="token">Cancellation token source.</param>
        /// <returns>Returns enumerable list.</returns>
        IObservable<T> ListEntitiesObservable<T>(string tableName, string filterQuery, CancellationTokenSource token = default)
            where T : class, ITableItem, new();

        /// <summary>
        /// List the entities of a given table name, with a supplied query.  Results returned as an Observable.
        /// </summary>
        /// <typeparam name="T">Type of object returned in the Enumerable.</typeparam>
        /// <param name="tableName">Name of the table to search within.</param>
        /// <param name="selectColumns">The columns to select (if required).</param>
        /// <param name="token">Cancellation token source.</param>
        /// <returns>Returns enumerable list.</returns>
        IObservable<T> ListEntitiesObservable<T>(string tableName, List<string> selectColumns, CancellationTokenSource token = default)
            where T : class, ITableItem, new();

        /// <summary>
        /// List the entities of a given table name, with a supplied query.  Results returned as an Observable.
        /// </summary>
        /// <typeparam name="T">Type of object returned in the Enumerable.</typeparam>
        /// <param name="tableName">Name of the table to search within.</param>
        /// <param name="selectColumns">The columns to select (if required).</param>
        /// <param name="filterQuery">The query to execute.</param>
        /// <param name="token">Cancellation token source.</param>
        /// <returns>Returns enumerable list.</returns>
        IObservable<T> ListEntitiesObservable<T>(string tableName, List<string> selectColumns, string filterQuery, CancellationTokenSource token = default)
            where T : class, ITableItem, new();

        /// <summary>
        /// List all the table names in the storage account. Results returned as an Enumerable.
        /// </summary>
        /// <returns>Returns string list of table names.</returns>
        Task<IEnumerable<string>> ListTableNames();

        /// <summary>
        /// Count items from a table filtering by query.
        /// </summary>
        /// <param name="tableName">Name of the table to count items from.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="token">Cancellation token source.</param>
        /// <returns>Number of items found.</returns>
        Task<long> CountItemsQuery(string tableName, string query, CancellationTokenSource token = default);

        /// <summary>
        /// Count items in a table, filtering by key.
        /// </summary>
        /// <param name="tableName">Name of the table to count items from.</param>
        /// <param name="key">Specific key to find</param>
        /// <param name="token">Cancellation token source.</param>
        /// <returns>Number of items found.</returns>
        Task<long> CountItems(string tableName, string key, CancellationTokenSource token = default);

        /// <summary>
        /// Count items in a table.
        /// </summary>
        /// <param name="tableName">Name of the table to count items from.</param>
        /// <param name="token">Cancellation token source.</param>
        /// <returns>Number of items found.</returns>
        Task<long> CountItems(string tableName, CancellationTokenSource token = default);

        /// <summary>
        /// Count items in a table, intercept the increment event.
        /// </summary>
        /// <param name="tableName">Name of the table to count items from.</param>
        /// <param name="countIncrement">Action, called every time an increment happens.</param>
        /// <param name="token">Cancellation token source.</param>
        /// <returns>Number of items found.</returns>
        Task<long> CountItems(string tableName, Action<long> countIncrement,  CancellationTokenSource token = default);

        /// <summary>
        /// Deletes the table from the storage account.
        /// </summary>
        /// <param name="tableName">The name of the table to be deleted.</param>
        /// <returns>Task.</returns>
        Task DeleteTable(string tableName);

        /// <summary>
        /// Creates a table in the storage account.
        /// </summary>
        /// <param name="tableName">The name of the table to be created.</param>
        /// <returns>Task.</returns>
        Task CreateTable(string tableName);
    }

    /// <summary>
    /// Interface ITableItem is used to ensure generic items passed in and out have at least got a key property.
    /// </summary>
    public interface ITableItem
    {
        /// <summary>
        /// Gets or sets the identifier key.
        /// </summary>
        /// <value>The key.</value>
        string Key { get; set; }
    }
}
