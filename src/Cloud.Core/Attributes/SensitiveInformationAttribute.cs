namespace Cloud.Core.Attributes
{
    using System;

    /// <summary>
    /// Sensitive Information Attribute can be used to mark properties that are secrets.
    /// Implements the <see cref="Attribute" />
    /// </summary>
    /// <seealso cref="Attribute" />
    public class SensitiveInfoAttribute : Attribute
    {
    }
}
