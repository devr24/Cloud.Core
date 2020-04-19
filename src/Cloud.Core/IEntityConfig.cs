namespace Cloud.Core
{
    /// <summary>
    /// A config with the necessary info on what should be done during entity creation/deletion etc.
    /// </summary>
    public interface IEntityConfig
    {
        /// <summary>
        /// Gets or sets the name of the entity that will be created.
        /// </summary>
        /// <value>
        /// The name of the entity.
        /// </value>
        string EntityName { get; set; }
    }
}
