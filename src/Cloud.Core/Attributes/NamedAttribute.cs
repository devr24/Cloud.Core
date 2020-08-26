namespace Cloud.Core.Attributes
{
    using System;

    /// <summary>
    /// Named Attribute can be used to mark properties with a specific name that can be looked up later.
    /// Implements the <see cref="Attribute" />
    /// </summary>
    /// <seealso cref="Attribute" />
    public class NamedAttribute : Attribute
    {
        /// <summary>Gets the name.</summary>
        public string Name { get; }

        /// <summary>Initializes a new instance of the <see cref="NamedAttribute"/> class.</summary>
        /// <param name="name">The name.</param>
        public NamedAttribute(string name)
        {
            Name = name;
        }
    }
}
