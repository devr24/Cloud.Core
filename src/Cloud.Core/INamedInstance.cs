namespace Cloud.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Contract for a Named Instance implementation.  Enables support for the Named Instance Factory.
    /// </summary>
    public interface INamedInstance
    {
        /// <summary>
        /// Gets or sets the name for the implementor of the INamedInstance interface.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }
    }

    /// <summary>
    /// Factory for getting instances of type T (where T is constrained to INamedInstance type).
    /// </summary>
    /// <typeparam name="T">Type T of a class that implemented INamedInstance.</typeparam>
    public class NamedInstanceFactory<T>
        where T : INamedInstance
    {
        /// <summary>
        /// Gets the object type T, using the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>T.</returns>
        /// <exception cref="ArgumentException">name</exception>
        public T this[string name]
        {
            get
            {
                if (Clients.TryGetValue(name, out var client))
                    return client;

                // If the client was not found, then throw exception, as this is not expected behaviour.
                throw new ArgumentException(nameof(name));
            }
        }

        /// <summary>
        /// Tries to get the instance.  Returns true if found and false if not.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if successfully found the value, <c>false</c> otherwise.</returns>
        public bool TryGetValue(string name, out T value)
        {
            if (Clients.TryGetValue(name, out var returnValue))
            {
                value = returnValue;
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>List of instances within the named instance factory.</summary>
        /// <returns>List&lt;T&gt;. List of instances in the factory.</returns>
        public List<T> GetInstances()
        {
            return Clients.Values.ToList();
        }

        /// <summary>List of instance names within the named instance factory.</summary>
        /// <returns>List&lt;System.String&gt;. List of named instance names.</returns>
        public List<string> GetInstanceNames()
        {
            return Clients.Keys.ToList();
        }

        internal readonly IDictionary<string, T> Clients = new Dictionary<string, T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedInstanceFactory{T}"/> class.
        /// </summary>
        /// <param name="clients">The clients.</param>
        public NamedInstanceFactory(IEnumerable<T> clients)
        {
            foreach (var client in clients)
            {
                var name = client.Name;

                // Default the name if it was not set.
                if (name.IsNullOrEmpty())
                {
                    name = client.GetType().Name;
                }

                // Auto-generate new name (rather than erroring at the moment - might change later) if it already exists.
                if (Clients.ContainsKey(name))
                {
                    var generatedName = GenerateName(name);
                    Clients.Add(generatedName, client);
                }
                else
                    Clients.Add(name, client);
            }
        }

        private string GenerateName(string suggestedName, int increment = 0)
        {
            increment++;
            var returnName = suggestedName + increment;

            if (Clients.ContainsKey(returnName))
                return GenerateName(suggestedName, increment);

            return returnName;
        }
    }
}
