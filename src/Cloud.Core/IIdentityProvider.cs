namespace Cloud.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Contract provides functions for Identity Providers.
    /// </summary>
    public interface IIdentityProvider : IDisposable
    {
        /// <summary>
        /// Lists the users.
        /// </summary>
        /// <param name="includeMemberOf">Optional parameter to populate the <see cref="IIdentity.MemberOf"/> field, will make the method call take longer.</param>
        /// <returns><see cref="System.Collections.IEnumerable"/> of <see cref="IIdentity"/> of type <see cref="IdentityType"/> "User".</returns>
        Task<IEnumerable<IIdentity>> GetUsers(bool includeMemberOf = false);

        /// <summary>
        /// Lists the groups.
        /// </summary>
        /// <param name="includeMemberOf">Optional parameter to populate the <see cref="IIdentity.MemberOf"/> field, will make the method call take longer.</param>
        /// <returns><see cref="System.Collections.IEnumerable"/> of <see cref="IIdentity"/> of type <see cref="IdentityType"/> "Group".</returns>
        Task<IEnumerable<IIdentity>> GetGroups(bool includeMemberOf = false);

        /// <summary>
        /// Lists the identities.
        /// </summary>
        /// <param name="includeMemberOf">Optional parameter to populate the <see cref="IIdentity.MemberOf"/> field, will make the method call take longer.</param>
        /// <returns><see cref="System.Collections.IEnumerable"/> of <see cref="IIdentity"/>.</returns>
        Task<IEnumerable<IIdentity>> GetIdentities(bool includeMemberOf = false);

        /// <summary>
        /// Returns the User identity that matches the Id.
        /// </summary>
        /// <param name="id">The Id of the User identity.</param>
        /// <param name="includeMemberOf">Optional parameter to populate the <see cref="IIdentity.MemberOf"/> field, will make the method call take longer.</param>
        /// <returns><see cref="IIdentity"/> of type <see cref="IdentityType"/> "User" that matches the Id.</returns>
        Task<IIdentity> GetUser(string id, bool includeMemberOf = false);

        /// <summary>
        /// Returns the Group identity that matches the Id.
        /// </summary>
        /// <param name="id">The Id of the Group identity.</param>
        /// <param name="includeMemberOf">Optional parameter to populate the <see cref="IIdentity.MemberOf"/> field, will make the method call take longer.</param>
        /// <returns><see cref="IIdentity"/> of type <see cref="IdentityType"/> "Group" that matches the Id.</returns>
        Task<IIdentity> GetGroup(string id, bool includeMemberOf = false);

        /// <summary>
        /// Returns the identity that matches the Id.
        /// </summary>
        /// <param name="id">The Id of the identity.</param>
        /// <param name="includeMemberOf">Optional parameter to populate the <see cref="IIdentity.MemberOf"/> field, will make the method call take longer.</param>
        /// <returns><see cref="IIdentity"/> that matches the Id.</returns>
        Task<IIdentity> GetIdentity(string id, bool includeMemberOf = false);

        /// <summary>
        /// Returns a list of Groups that the User matching the input Id is a member of.
        /// </summary>
        /// <param name="id">The Id of the User identity.</param>
        /// <returns><see cref="System.Collections.IEnumerable"/> of <see cref="IIdentity"/> of type <see cref="IdentityType"/> "Group" that the matching User is a member of.</returns>
        Task<IEnumerable<IIdentity>> GetUsersGroupMemberships(string id);
    }

    /// <summary>
    /// Contract of a common Identity item.
    /// </summary>
    public interface IIdentity
    {
        /// <summary>
        /// Gets the Id of the Identity.
        /// </summary>
        /// <value>
        /// The Id of the Identity.
        /// </value>
        string Id { get; }

        /// <summary>
        /// Gets the Display Name of the Identity.
        /// </summary>
        /// <value>
        /// The Display Name of the Identity.
        /// </value>
        string DisplayName { get; }

        /// <summary>
        /// Gets the Type of Identity.
        /// </summary>
        /// <value>
        /// The Type of Identity.
        /// </value>
        IdentityType Type { get; }

        /// <summary>
        /// Gets the Email address of the Identity.
        /// </summary>
        /// <value>
        /// The Email address of the Identity.
        /// </value>
        string Email { get; }

        /// <summary>
        /// Gets the MemberOf Id collection of the Identity.
        /// </summary>
        /// <value>
        /// Collection of Identity Ids that the Identity is a member of.
        /// </value>
        HashSet<string> MemberOf { get; }
    }

    /// <summary>
    /// The possible types for an IIdentity.
    /// </summary>
    public enum IdentityType
    {
        All = -1,
        User,
        Group
    }
}
