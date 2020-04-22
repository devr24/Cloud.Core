namespace Cloud.Core
{
    /// <summary>
    /// Interface IFeatureFlag
    /// </summary>
    public interface IFeatureFlag
    {
        /// <summary>
        /// Gets the feature flag value.
        /// </summary>
        /// <param name="key">The key for the feature flag.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns><c>true</c> if feature flag is set to true, <c>false</c> otherwise.</returns>
        bool GetFeatureFlag(string key, bool defaultValue = false);
    }
}
