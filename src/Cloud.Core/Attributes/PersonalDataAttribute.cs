namespace Cloud.Core.Attributes
{
    using System;

    /// <summary>
    /// Personal Data Attribute can be used to mark information as Personal Identifiable Information (Pii) data.
    /// Implements the <see cref="Attribute" />
    /// </summary>
    /// <seealso cref="Attribute" />
    public class PersonalDataAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="PersonalDataAttribute"/> class.</summary>
        public PersonalDataAttribute() : base()
        {

        }
    }
}
