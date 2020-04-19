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

        /// <summary>
        /// Gets or sets the Last Write Time of the blob.
        /// </summary>
        /// <value>
        /// The Last Write Time of the file.
        /// </value>
        DateTime LastWriteTime { get; }

        /// <summary>
        /// Gets the properties (metadata) of the blob item.
        /// </summary>
        /// <value>The properties to set for the blob item.</value>
        Dictionary<string, string> Properties { get; set; }

        /// <summary>
        /// Gets the custom metadata of the blob item
        /// </summary>
        Dictionary<string, string> Metadata { get; set; }
    }

    /// <summary>
    /// Contract provides functions for BLOB stores.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IBlobStorage : INamedInstance, IDisposable
    {
        /// <summary>
        /// Lists the root folders from storage.
        /// </summary>
        /// <returns><see cref="System.Collections.IEnumerable"/> of <see cref="System.string"/> root folder names.</returns>
        Task<IEnumerable<string>> ListFolders();

        /// <summary>Gets the BLOB item (no lock).</summary>
        /// <param name="blobPath">The BLOB path including name.</param>
        /// <param name="fetchAttributes">if set to <c>true</c> [will fetch additional attributes].</param>
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
        /// Check if the specified BLOB exists in storage.
        /// </summary>
        /// <param name="blobPath">The BLOB path to check for.</param>
        /// <returns>Task&lt;System.Boolean&gt; [true] if exists and [false] if not.</returns>
        Task<bool> Exists(string blobPath);

        /// <summary>
        /// Unlocks the BLOB item.
        /// </summary>
        /// <param name="item">The BLOB item to unlock.</param>
        void UnlockBlob(IBlobItem item);

        /// <summary>Lists all BLOB items from storage in the path requested.</summary>
        /// <param name="rootFolder">The root folder to traverse.</param>
        /// <param name="recursive">If set to <c>true</c> then [recursively] check subfolders.</param>
        /// <param name="fetchAttributes">if set to <c>true</c> [will fetch additional attributes].</param>
        /// <param name="searchPrefix">File name prefix used to filter the search.</param>
        /// <returns>[System.Collections.IEnumerable] of IBlobItem all BLOB items in the path requested.</returns>
        IEnumerable<IBlobItem> ListBlobs(string rootFolder, bool recursive, bool fetchAttributes = false, string searchPrefix = null);

        /// <summary>
        /// Lists the BLOB items from storage, in the path requested, as an observable.
        /// </summary>
        /// <param name="rootFolder">The root folder to traverse.</param>
        /// <param name="recursive">If set to <c>true</c> then [recursively] check subfolders.</param>
        /// <param name="fetchAttributes">if set to <c>true</c> [will fetch additional attributes].</param>
        /// <param name="searchPrefix">File name prefix used to filter the search.</param>
        /// <returns><see cref="IObservable{IBlobItem}" /> observable for each blob item.</returns>
        IObservable<IBlobItem> ListBlobsObservable(string rootFolder, bool recursive, bool fetchAttributes = false, string searchPrefix = null);

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
        /// <param name="metaData">Dictionary of additional metadata to append to the blob stored.</param>
        /// <returns>Async Task</returns>
        Task UploadBlob(string blobPath, Stream stream, Dictionary<string, string> metaData = null);

        /// <summary>
        /// Upload the BLOB to storage.  BLOB is read from the passed in file path.
        /// </summary>
        /// <param name="blobPath">The BLOB path.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="metaData">Dictionary of additional metadata to append to the blob stored.</param>
        /// <returns>Async Task</returns>
        Task UploadBlob(string blobPath, string filePath, Dictionary<string, string> metaData = null);

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

        /// <summary>
        /// Adds metadata to the BLOB
        /// </summary>
        /// <param name="blob">The BLOB path</param>
        /// <returns>Task.</returns>
        Task UpdateBlobMetadata(IBlobItem blob);

        /// <summary>
        /// Copies the content of one directory to another on the server side.
        /// Avoids having to download all items and reupload them to somewhere else on the client side.
        /// </summary>
        /// <param name="sourceDirectoryPath"></param>
        /// <param name="destinationDirectoryPath"></param>
        /// <param name="transferEvent"></param>
        /// <returns>Result from the  server side transfer.</returns>
        Task<ITransferResult> CopyDirectory(string sourceDirectoryPath, string destinationDirectoryPath,
            Action<TransferEventType, ITransferEvent> transferEvent = null);

        /// <summary>
        /// Gets a Signed Access Url to the specified blob with the provided Access Config
        /// </summary>
        /// <param name="blobPath">Path to the blob we are requesting access to</param>
        /// <param name="signedAccessConfig">Access config including required permissions and Access URL Expiry</param>
        /// <returns>The signed access URL for the specified blob.</returns>
        Task<string> GetSignedBlobAccessUrl(string blobPath, ISignedAccessConfig signedAccessConfig);

        /// <summary>
        /// Gets a Signed Access Url to the specified folder with the provided Access Config
        /// </summary>
        /// <param name="folderPath">Full Path to the Folder we want to include in the Access URL</param>
        /// <param name="signedAccessConfig">Access config including required permissions and Access URL Expiry</param>
        /// <returns>The signed access URL for the specified Folder.</returns>
        Task<string> GetSignedFolderAccessUrl(string folderPath, ISignedAccessConfig signedAccessConfig);
    }

    /// <summary>
    ///  Contract of a common Transfer Result from server side directory transfer.
    /// </summary>
    public interface ITransferResult
    {
        /// <summary>
        /// Gets the number of bytes that have been transferred.
        /// </summary>
        long BytesTransferred { get; }

        /// <summary>
        /// Gets the number of files that have been transferred.
        /// </summary>
        long NumberOfFilesTransferred { get; }

        /// <summary>
        /// Gets the number of files that are skipped to be transferred.
        /// </summary>
        long NumberOfFilesSkipped { get; }

        /// <summary>
        /// Gets the number of files that are failed to be transferred.
        /// </summary>
        long NumberOfFilesFailed { get; }
    }


    /// <summary>
    ///  Contract of a common Transfer Event for server side transfer 
    /// </summary>
    public interface ITransferEvent
    {
        /// <summary>Gets the instance representation of transfer source location.</summary>
        object Source { get; set; }

        /// <summary>Gets the instance representation of transfer destination location.</summary>
        object Destination { get; set; }

        /// <summary>Gets transfer start time.</summary>
        DateTime StartTime { get; set; }

        /// <summary>Gets transfer end time.</summary>
        DateTime EndTime { get; set; }

        /// <summary>Gets the exception if the transfer is failed, or null if the transfer is success.</summary>
        Exception Exception { get; set; }
    }


    /// <summary>
    /// Type of event that took place.
    /// </summary>
    public enum TransferEventType
    {
        /// <summary>Failed the transfer</summary>
        Failed,
        /// <summary>Tranferred successfully</summary>
        Transferred,
        /// <summary>Skipped the transfer</summary>
        Skipped
    }
}
