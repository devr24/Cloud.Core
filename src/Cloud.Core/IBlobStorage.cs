namespace Cloud.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Contract of a common BLOB item.
    /// </summary>
    public interface IBlobItem
    {
        /// <summary>
        /// Gets the BLOB file extension.
        /// </summary>
        /// <value>
        /// The BLOB file extension.
        /// </value>
        string FileExtension { get; }

        /// <summary>
        /// Gets the name of the BLOB file.
        /// </summary>
        /// <value>
        /// The name of the BLOB file.
        /// </value>
        string FileName { get; }

        /// <summary>
        /// Gets the file name without extension.
        /// </summary>
        /// <value>The file name without extension.</value>
        string FileNameWithoutExtension { get; }

        /// <summary>
        /// Gets the full relative path to the BLOB stored.
        /// </summary>
        /// <value>
        /// The path to the BLOB stored.
        /// </value>
        string Path { get; }

        /// <summary>Gets the size of the file.</summary>
        /// <value>The size of the file.</value>
        long FileSize { get; }

        /// <summary>Content Hash representing the file content.</summary>
        string ContentHash { get; }

        /// <summary>
        /// Gets the root level parent folder where the BLOB is held.
        /// </summary>
        /// <value>
        /// The root folder name.
        /// </value>
        string RootFolder { get; }

        /// <summary>
        /// The tag object will hold the concrete class from which the
        /// specialized version of BLOB item is stored.
        /// </summary>
        /// <value>
        /// The tag object.
        /// </value>
        object Tag { get; }

        /// <summary>
        /// Gets or sets the name of the unique lease.
        /// </summary>
        /// <value>
        /// The name of the unique lease.
        /// </value>
        string UniqueLeaseName { get; set; }
    }

    /// <summary>
    /// Contract provides functions for BLOB stores.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IBlobStorage : IDisposable
    {
        /// <summary>
        /// Lists the root folders from storage.
        /// </summary>
        /// <returns><see cref="System.Collections.IEnumerable"/> of <see cref="System.string"/> root folder names.</returns>
        Task<IEnumerable<string>> ListFolders();

        /// <summary>Gets the BLOB item (no lock).</summary>
        /// <param name="blobPath">The BLOB path including name.</param>
        /// <param name="fetchAttributes">if set to <c>true</c> [fetch attributes].</param>
        /// <returns>BLOB item found.</returns>
        Task<IBlobItem> GetBlob(string blobPath, bool fetchAttributes = false);

        /// <summary>
        /// Gets the BLOB with a lock.  This stops the BLOB from being modified whilst locked.
        /// </summary>
        /// <param name="blobPath">The path to the BLOB.</param>
        /// <param name="leaseName">Name of the lease.</param>
        /// <returns>
        ///   <see cref="IBlobItem" /> found.
        /// </returns>
        Task<IBlobItem> GetBlobWithLock(string blobPath, string leaseName = "");

        /// <summary>
        /// Unlocks the BLOB item.
        /// </summary>
        /// <param name="item">The BLOB item to unlock.</param>
        void UnlockBlob(IBlobItem item);

        /// <summary>
        /// Lists all BLOB items from storage in the path requested.
        /// </summary>
        /// <param name="rootFolder">The root folder to traverse.</param>
        /// <param name="recursive">If set to <c>true</c> then [recursively] check subfolders.</param>
        /// <returns><see cref="System.Collections.IEnumerable"/> of <see cref="IBlobItem"/> all BLOB items in the path requested.</returns>
        IEnumerable<IBlobItem> ListBlobs(string rootFolder, bool recursive);

        /// <summary>
        /// Lists the BLOB items from storage, in the path requested, as an observable.
        /// </summary>
        /// <param name="rootFolder">The root folder to traverse.</param>
        /// <param name="recursive">If set to <c>true</c> then [recursively] check subfolders.</param>
        /// <returns><see cref="IObservable{IBlobItem}"/> observable for each blob item.</returns>
        IObservable<IBlobItem> ListBlobsObservable(string rootFolder, bool recursive);

        /// <summary>
        /// Downloads the BLOB in the path from storage to a memory stream.
        /// </summary>
        /// <param name="blobPath">The BLOB path to download.</param>
        /// <returns><see cref="Stream"/> containing the downloaded BLOB.</returns>
        Task<Stream> DownloadBlob(string blobPath);

        /// <summary>
        /// Downloads the BLOB in the path from storage to the local file system.
        /// </summary>
        /// <param name="blobPath">The BLOB path to download.</param>
        /// <param name="filePath">The file path to save to.</param>
        /// <returns>Async Task</returns>
        Task DownloadBlob(string blobPath, string filePath);

        /// <summary>
        /// Downloads the BLOB.
        /// </summary>
        /// <param name="blob">The BLOB item to download content for.</param>
        /// <returns>Memory stream of downloaded blob content.</returns>
        Task<Stream> DownloadBlob(IBlobItem blob);

        /// <summary>
        /// Upload the BLOB to storage.  BLOB is read from the passed in stream.
        /// </summary>
        /// <param name="blobPath">The BLOB path.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>Async Task</returns>
        Task UploadBlob(string blobPath, Stream stream);

        /// <summary>
        /// Upload the BLOB to storage.  BLOB is read from the passed in file path.
        /// </summary>
        /// <param name="blobPath">The BLOB path.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns>Async Task</returns>
        Task UploadBlob(string blobPath, string filePath);

        /// <summary>
        /// Deletes the BLOB from storage.
        /// </summary>
        /// <param name="blobPath">The BLOB path to delete.</param>
        /// <returns>Async Task</returns>
        Task DeleteBlob(string blobPath);

        /// <summary>
        /// Removes the folder at the designated path (and all content within).
        /// </summary>
        /// <param name="path">The path to remove.</param>
        /// <returns>Task.</returns>
        Task RemoveFolder(string path);

        /// <summary>
        /// Adds the folder at the designated path.
        /// </summary>
        /// <param name="path">The path to add.</param>
        /// <returns>Task.</returns>
        Task AddFolder(string path);
    }
}