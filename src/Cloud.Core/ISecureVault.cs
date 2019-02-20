namespace Cloud.Core
{
    using JetBrains.Annotations;
    using System.Threading.Tasks;
    
    /// <summary>
    /// Contract specifying the functionality of a Secure Vault.
    /// Kept to Get and Set accessors for secrets for simplicity.
    /// </summary>
    public interface ISecureVault
    {
        /// <summary>
        /// Sets the secret in the secret vault implementation.
        /// </summary>
        /// <param name="secretName">Name of the secret to set.</param>
        /// <param name="secretValue">The value for the secret.</param>
        /// <returns></returns>
        Task SetSecret([NotNull] string secretName, [NotNull] string secretValue);

        /// <summary>
        /// Gets the secret from the secret vault implementation.
        /// </summary>
        /// <param name="secretName">Name of the secret to retrieve.</param>
        /// <returns></returns>
        Task<string> GetSecret([NotNull] string secretName);
    }
}
