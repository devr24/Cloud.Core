// ReSharper disable once CheckNamespace
namespace System
{
    using Collections;
    using Collections.Generic;
    using ComponentModel.DataAnnotations;
    using Linq;
    using Reflection;
    using Newtonsoft.Json;

    /// <summary>
    /// Extension methods for Type.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether or not the type is a system type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>[true] if is a system type, otherwise [false].</returns>
        public static bool IsSystemType(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            return
                type.IsPrimitive ||
                new [] {
                    typeof(string),
                    typeof(decimal),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid)
                }.Contains(type) ||
                type.IsEnum ||
                Convert.GetTypeCode(type) != TypeCode.Object ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSystemType(type.GetGenericArguments().FirstOrDefault()));
        }

        /// <summary>
        /// Determines whether the type can be enumerated.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>[true] if is a enumerable type, otherwise [false].</returns>
        public static bool IsEnumerableType(this Type type)
        {
            if (type == null || type.IsSystemType())
            {
                return false;
            }

            return type.IsArray || type.GetInterfaces().Intersect(new [] {
                       typeof(IList),
                       typeof(ICollection),
                       typeof(IEnumerable),
                       typeof(ICollection),
                   }).Any() ||
                   typeof(IEnumerable<object>).IsAssignableFrom(type);
        }

        /// <summary>
        /// Get a list of properties with the required attribute.
        /// </summary>
        /// <param name="type">Type object to check properties.</param>
        /// <returns>List of required props.</returns>
        public static IEnumerable<PropertyInfo> GetRequiredProperties(this Type type)
        {
            return from prop in type.GetProperties()
                   where prop.IsRequiredProperty()
                   select prop;
        }

        /// <summary>
        /// Determines if the property info contains required attributes.
        /// Checks "Required", "JsonRequired" and "JsonProperty(Required)" attributes.
        /// </summary>
        /// <param name="prop">Property info to check.</param>
        /// <returns>True if contains a required attribute.</returns>
        public static bool IsRequiredProperty(this PropertyInfo prop)
        {
            // Check each custom attribute of the property for the "Required" attribute.
            foreach (var att in prop.CustomAttributes)
            {
                // Data annotation required.
                if (att.AttributeType == typeof(RequiredAttribute))
                {
                    return true;
                }

                // Json required annotation.
                if (att.AttributeType == typeof(JsonRequiredAttribute))
                {
                    return true;
                }

                // Json property (required) annotation.
                if (att.AttributeType == typeof(JsonPropertyAttribute))
                {
                    var requiredArg = att.NamedArguments?.Where(a => a.MemberName == "Required").FirstOrDefault();
                    var val = requiredArg?.TypedValue.Value;

                    if (val != null)
                    {
                        if ((Required)val == Required.Always || (Required)val == Required.DisallowNull)
                            return true;
                    }
                }
            }

            return false;
        }
    }
}
