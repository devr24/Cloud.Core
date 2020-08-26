// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    using ComponentModel;
    using ComponentModel.DataAnnotations;
    using Reflection;

    /// <summary>
    /// Extension methods for generic type T
    /// </summary>
    public static class GenericTypeExtensions
    {
        /// <summary>
        /// Gets the identity field.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>System.Object.</returns>
        public static object GetIdentityField<T>(this T value)
        {
            var identity = value.GetType().GetIdentityProperty();
            return identity.GetValue(value);
        }

        /// <summary>
        /// Changes the type of an object to a desired type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>T.</returns>
        public static T ChangeType<T>(this object value)
        {
            if (value is T variable) return variable;

            try
            {
                // Handling Nullable types i.e, int?, double?, bool? .. etc
                if (Nullable.GetUnderlyingType(typeof(T)) != null)
                {
                    return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);
                }

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// Checks whether the given instance is <c>null</c> or empty.
        /// </summary>
        /// <typeparam name="T">Type of instance.</typeparam>
        /// <param name="instance">Instance to check.</param>
        /// <returns>Returns <c>True</c>, if the original instance is <c>null</c> or empty; otherwise returns <c>False</c>.</returns>
        public static bool IsNullOrDefault<T>(this T instance)
        {
            return instance == null || instance.Equals(default(T));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the given instance is <c>null</c> or empty.
        /// </summary>
        /// <typeparam name="T">Type of instance.</typeparam>
        /// <param name="instance">Instance to check.</param>
        /// <returns>Returns the original instance, if the instance is NOT <c>null</c>; otherwise throws an <see cref="ArgumentNullException"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="instance"/> is <see langword="null"/></exception>
        public static T ThrowIfNullOrDefault<T>(this T instance)
        {
            if (instance.IsNullOrDefault())
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance;
        }

        /// <summary>
        /// Take a sub-array from a given array.
        /// </summary>
        /// <typeparam name="T">Type of instance.</typeparam>
        /// <param name="data">Original array that the sub-array will be generated from.</param>
        /// <param name="index">Index to start the sub-array.</param>
        /// <param name="length">Number of elements to take from start index.</param>
        /// <returns>Type T sub-array.</returns>
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        /// <summary>
        /// Builds a list of property descriptions for each public property on the object type T.
        /// </summary>
        /// <typeparam name="T">Instance of the object to gather properties (and values) from.</typeparam>
        /// <param name="obj">Object to gather properties from.</param>
        /// <returns>List of property description objects.</returns>
        public static IEnumerable<PropertyDescription> GetPropertyDescription<T>(this T obj)
        {
            var items = obj.GetType().GetProperties();

            foreach (var desc in items)
            {
                var val = desc.GetValue(obj);
                yield return new PropertyDescription(desc, val);
            }
        }
    }

    /// <summary>
    /// Class that describes a given object.
    /// </summary>
    public class PropertyDescription
    {
        /// <summary>
        /// Property descriptions can only be created within the Cloud.Core package, therefore the constructor is internal.
        /// </summary>
        /// <param name="propertyInfo">Information about the property being described.</param>
        /// <param name="value">The value of the property being described.</param>
        internal PropertyDescription(PropertyInfo propertyInfo, object value)
        {
            Type = propertyInfo.PropertyType;
            Name = propertyInfo.Name;
            Value = value;
            IsSystemType = Type.IsSystemType();
            IsEnumerable = !IsSystemType && Type.IsEnumerableType();
            HasRequiredAttribute = propertyInfo.IsRequiredProperty();
            HasKeyAttribute = Attribute.IsDefined(propertyInfo, typeof(KeyAttribute));
        }

        /// <summary>
        /// Type of the object being described.
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// Name of the object being described.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Whether the object is a system type [true] or not [false].  System types are strings, bool, int etc.
        /// </summary>
        public bool IsSystemType { get; }

        /// <summary>
        /// Value of the object being described.
        /// </summary>
        public object Value { get; internal set; }

        /// <summary>
        /// Whether the object is an enumerable type.
        /// </summary>
        public bool IsEnumerable { get; }

        /// <summary>
        /// Whether the object has the "Key" attribute associated with it.
        /// </summary>
        public bool HasKeyAttribute { get; }

        /// <summary>
        /// Whether the object has the "Required", "JsonRequired" or "JsonProperty(Required=true)" attribute associated with it.
        /// </summary>
        public bool HasRequiredAttribute { get; }
    }
}
