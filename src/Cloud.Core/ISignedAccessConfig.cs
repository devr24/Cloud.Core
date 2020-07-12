namespace Cloud.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface containing Access Config, permissions required and Access Expiry
    /// </summary>
    public interface ISignedAccessConfig
    {
        /// <summary>
        /// List of permissions required for Access URL
        /// </summary>
        List<AccessPermission> AccessPermissions { get; set; }

        /// <summary>
        /// Expiry time of the Access URL
        /// </summary>
        DateTimeOffset? AccessExpiry { get; set; }
    }

    /// <summary>
    /// List of available permissions for accessing items within Storage.
    /// </summary>
    public enum AccessPermission
    {
        /// <summary>
        /// No access.
        /// </summary>
        None,
        /// <summary>
        /// Read access.
        /// </summary>
        Read,
        /// <summary>
        /// Write access.
        /// </summary>
        Write,
        /// <summary>
        /// Delete access.
        /// </summary>
        Delete,
        /// <summary>
        /// List information access.
        /// </summary>
        List,
        /// <summary>
        /// Add access.
        /// </summary>
        Add,
        /// <summary>
        /// Create access.
        /// </summary>
        Create,
        /// <summary>
        /// Update access.
        /// </summary>
        Update
    }
}
