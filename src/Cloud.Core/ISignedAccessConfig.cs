namespace Cloud.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class containing Access Config, permissions required and Access Expiry
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
    /// Class to allow us to pass permission and expiry properties to Blob Storage to retrieve Access Url
    /// </summary>
    public class SignedAccessConfig : ISignedAccessConfig
    {
        /// <summary>
        /// List of Permissions the Access URL should have
        /// </summary>
        public List<AccessPermission> AccessPermissions { get; set; }

        /// <summary>
        /// Required Expiry for the Access URL
        /// </summary>
        public DateTimeOffset? AccessExpiry { get; set; }

        /// <summary>
        /// Instantiate a new instance of the SignedAccessConfig with the supplied parameters
        /// </summary>
        /// <param name="accessPermissions">Permissions requested for the Access URL</param>
        /// <param name="accessExpiry">Expiry for the Access URL</param>
        public SignedAccessConfig(List<AccessPermission> accessPermissions, DateTimeOffset? accessExpiry)
        {
            AccessPermissions = accessPermissions;
            AccessExpiry = accessExpiry;
        }
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
