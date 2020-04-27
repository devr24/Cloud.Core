namespace Cloud.Core.Validation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Exceptions;

    /// <summary>
    /// Class AttributeValidator - use this when model validation will be used in the inheriting class.
    /// </summary>
    public abstract class AttributeValidator
    {
        /// <summary>
        /// Returns validate result this instance. All properties in the inheriting class will be validated.  Decorate
        /// them using DataAnnotations, such as "Required" or "MaxLength".
        /// </summary>
        /// <param name="serviceProvider">Existing service provider - typically used to resolve IStringLocalizer.</param>
        /// <returns>ValidateResult representing the valid state of the inheriting object.</returns>
        public virtual ValidateResult Validate(IServiceProvider serviceProvider = null)
        {
            var context = new ValidationContext(this, serviceProvider: serviceProvider, items: null);
            var errorResults = new List<ValidationResult>();
            
            // Carry out validation.
            _ = Validator.TryValidateObject(this, context, errorResults, true);
            
            // Return result of validation.
            return new ValidateResult(errorResults);
        }

        /// <summary>
        /// Throws validation exception if invalid.
        /// </summary>
        /// <exception cref="ValidationException"></exception>
        public void ThrowIfInvalid()
        {
            var validationResult = Validate();
            if (!validationResult.IsValid)
            {
                throw new ValidateException(validationResult);
            }
        }
    }

    /// <summary>
    /// Class ValidateResult.
    /// </summary>
    public class ValidateResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateResult" /> class.
        /// </summary>
        /// <param name="errors">The error collection.</param>
        public ValidateResult(IEnumerable<ValidationResult> errors)
        {
            Errors = errors;
        }

        /// <summary>
        /// Returns true if there are no errors.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public bool IsValid => !Errors.Any();

        /// <summary>
        /// Gets the error collection.
        /// </summary>
        /// <value>The errors.</value>
        public IEnumerable<ValidationResult> Errors { get; internal set; }
    }
}
