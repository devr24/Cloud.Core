namespace Cloud.Core
{
    using System;

    /// <summary>
    /// Interface IAuthentication
    /// </summary>
    public interface IAuthentication : INamedInstance
    {
        /// <summary>
        /// Property to access authentication token
        /// </summary>
        /// <value>The access token.</value>
        IAccessToken AccessToken { get; }
    }

    /// <summary>
    /// Interface IAuthenticated
    /// </summary>
    public interface IAccessToken
    {
        /// <summary>
        /// Bearer token generated for authentication.
        /// </summary>
        /// <value>The bearer token.</value>
        string BearerToken { get; set; }

        /// <summary>
        /// The DateTimeOffset for bearer token expiry.
        /// </summary>
        /// <value>The expires.</value>
        DateTimeOffset Expires { get; set; }

        /// <summary>Gets a value indicating whether this authenticated instance has expired.</summary>
        /// <value><c>true</c> if this instance has expired; otherwise, <c>false</c>.</value>
        bool HasExpired { get; }
    }
}
