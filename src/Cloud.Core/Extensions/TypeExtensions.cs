// ReSharper disable once CheckNamespace
namespace System
{
    using Collections;
    using Collections.Generic;
    using ComponentModel.DataAnnotations;
    using Linq;
    using Reflection;
    using Newtonsoft.Json;
    using Cloud.Core.Attributes;

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
        /// Determines whether this type contains [has pii data].
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>True</c> if the specified type has Pii data; otherwise, <c>false</c>.</returns>
        public static bool HasPiiData(this Type type)
        {
            return type.GetProperties().Any(p => p.IsPiiData());
        }

        /// <summary>
        /// Determines whether this type contains [has sensitive information].
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>True</c> if the specified type has Pii data; otherwise, <c>false</c>.</returns>
        public static bool HasSensitiveInfo(this Type type)
        {
            return type.GetProperties().Any(p => p.IsSensitiveInfo());
        }

        /// <summary>
        ///Gets a list of Pii Data Fields.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>Dictionary of PiiData properties.</returns>
        public static Dictionary<string, PropertyInfo> GetPiiDataProperties(this Type type)
        {
            var items = new Dictionary<string, PropertyInfo>();
            foreach (var item in type.GetProperties().Where(p => p.IsPiiData()))
            {
                items.Add(item.Name, item);
            }
            return items;
        }

        /// <summary>
        ///Gets a list of Sensitive Information Fields.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>Dictionary of PiiData properties.</returns>
        public static Dictionary<string, PropertyInfo> GetSensitiveInfoProperties(this Type type)
        {
            var items = new Dictionary<string, PropertyInfo>();
            foreach (var item in type.GetProperties().Where(p => p.IsSensitiveInfo()))
            {
                items.Add(item.Name, item);
            }
            return items;
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

        /// <summary>
        /// Determines whether the specified property [is pii data].
        /// </summary>
        /// <param name="prop">The property to check.</param>
        /// <returns>System.Boolean true if has PersonalData attribute.</returns>
        public static bool IsPiiData(this PropertyInfo prop)
        {
            foreach (var att in prop.CustomAttributes)
            {
                if (att.AttributeType == typeof(PersonalDataAttribute))
                {
                    return true;
                }
                if (att.AttributeType.Name.Contains("PersonalData"))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified property [contains sensitive information].
        /// </summary>
        /// <param name="prop">The property to check.</param>
        /// <returns>System.Boolean true if has SensitiveInformation attribute.</returns>
        public static bool IsSensitiveInfo(this PropertyInfo prop)
        {
            return Attribute.IsDefined(prop, typeof(SensitiveInfoAttribute));
        }

        /// <summary>
        /// Determines whether the specified property [contains sensitive information].
        /// </summary>
        /// <param name="prop">The property to check.</param>
        /// <returns>System.Boolean true if has SensitiveInformation attribute.</returns>
        internal static bool IsSensitiveOrPersonalData(this PropertyInfo prop)
        {
            foreach (var att in prop.CustomAttributes)
            {
                if (att.AttributeType == typeof(PersonalDataAttribute) || att.AttributeType == typeof(SensitiveInfoAttribute))
                {
                    return true;
                }
                if (att.AttributeType.Name.Contains("PersonalData"))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///Gets a default value for the passed in property type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>Dictionary of PiiData properties.</returns>
        internal static object GetDefault(this Type prop)
        {
            return prop.IsValueType ? Activator.CreateInstance(prop) : null;
        }
    }
}
